using Lab10;

namespace Lab10Test.Purple
{
    [TestClass]
    public sealed class GeneralTest
    {
        private class TestManager : Lab10.Purple.PurpleFileManager<Lab9.Purple.Purple>
        {
            public TestManager(string name) : base(name) { }

            public override void Serialize(Lab9.Purple.Purple obj)
            {
            }

            public override Lab9.Purple.Purple Deserialize()
            {
                return null;
            }
        }

        [TestMethod]
        public void Test_00_OOP_IFileManager()
        {
            var fileManagerInterface = typeof(IFileManager);

            Assert.IsTrue(fileManagerInterface.IsInterface);

            Assert.IsNotNull(fileManagerInterface.GetProperty("FolderPath"));
            Assert.IsNotNull(fileManagerInterface.GetProperty("FileName"));
            Assert.IsNotNull(fileManagerInterface.GetProperty("FileExtension"));
            Assert.IsNotNull(fileManagerInterface.GetProperty("FullPath"));

            Assert.IsNotNull(fileManagerInterface.GetMethod("SelectFolder", new[] { typeof(string) }));
            Assert.IsNotNull(fileManagerInterface.GetMethod("SelectFile", new[] { typeof(string) }));
            Assert.IsNotNull(fileManagerInterface.GetMethod("ChangeFormat", new[] { typeof(string) }));
        }

        [TestMethod]
        public void Test_01_OOP_IFileLifeController()
        {
            var lifeInterface = typeof(IFileLifeController);

            Assert.IsTrue(lifeInterface.IsInterface);

            Assert.IsNotNull(lifeInterface.GetMethod("CreateFile"));
            Assert.IsNotNull(lifeInterface.GetMethod("EditFile", new[] { typeof(string) }));
            Assert.IsNotNull(lifeInterface.GetMethod("ChangeFileExtension", new[] { typeof(string) }));
            Assert.IsNotNull(lifeInterface.GetMethod("DeleteFile"));
        }

        [TestMethod]
        public void Test_02_OOP_ISerializer_T()
        {
            var serializerType = typeof(ISerializer<>);

            Assert.IsTrue(serializerType.IsInterface);

            var methods = serializerType.GetMethods();

            Assert.IsTrue(methods.Any(m => m.Name == "Serialize"));
            Assert.IsTrue(methods.Any(m => m.Name == "Deserialize"));
        }

        [TestMethod]
        public void Test_03_OOP_MyFileManager()
        {
            var managerType = typeof(MyFileManager);

            Assert.IsTrue(managerType.IsAbstract);

            Assert.IsTrue(typeof(IFileManager).IsAssignableFrom(managerType));
            Assert.IsTrue(typeof(IFileLifeController).IsAssignableFrom(managerType));
        }

        [TestMethod]
        public void Test_04_OOP_PurpleFileManager()
        {
            var PurpleType = typeof(Lab10.Purple.PurpleFileManager<>);

            Assert.IsTrue(PurpleType.IsAbstract);
            Assert.IsTrue(PurpleType.IsClass);

            Assert.IsTrue(PurpleType.IsSubclassOf(typeof(MyFileManager)));

            // проверка ISerializer<T>
            var interfaces = PurpleType.GetInterfaces();
            Assert.IsTrue(interfaces.Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(ISerializer<>)
            ), "PurpleFileManager must implement ISerializer<T>");

            // методы
            var serialize = PurpleType.GetMethod("Serialize");
            Assert.IsNotNull(serialize);
            Assert.IsTrue(serialize.IsAbstract);

            var deserialize = PurpleType.GetMethod("Deserialize");
            Assert.IsNotNull(deserialize);
            Assert.IsTrue(deserialize.IsAbstract);

            var edit = PurpleType.GetMethod("EditFile");
            Assert.IsTrue(edit.IsVirtual);
            Assert.IsFalse(edit.IsAbstract);

            var changeExt = PurpleType.GetMethod("ChangeFileExtension");
            Assert.IsTrue(changeExt.IsVirtual);
            Assert.IsFalse(changeExt.IsAbstract);
        }

        [TestMethod]
        public void Test_05_Inheritance()
        {
            var fileManagerInterface = typeof(IFileManager);
            var lifeInterface = typeof(IFileLifeController);
            var managerType = typeof(MyFileManager);

            Assert.IsTrue(fileManagerInterface.IsAssignableFrom(managerType));
            Assert.IsTrue(lifeInterface.IsAssignableFrom(managerType));
        }

        [TestMethod]
        public void Test_06_FileManager_Setup()
        {
            var manager = (IFileManager)new TestManager("test");

            var folder = Directory.GetCurrentDirectory();

            manager.SelectFolder(folder);
            manager.ChangeFileName("task");

            Assert.AreEqual(folder, manager.FolderPath);
            Assert.AreEqual("task", manager.FileName);
            Assert.IsTrue(manager.FullPath.Contains("task"));
        }

        [TestMethod]
        public void Test_07_FileCreation()
        {
            var manager = (IFileManager)new TestManager("test");

            var folder = Directory.GetCurrentDirectory();
            Directory.CreateDirectory(folder);

            manager.SelectFolder(folder);
            manager.ChangeFileName("task");

            ((IFileLifeController)manager).CreateFile();

            Assert.IsTrue(File.Exists(manager.FullPath));
        }

        [TestMethod]
        public void Test_08_ChangeFormat()
        {
            var manager = (IFileManager)new TestManager("test");

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "PurpleFormatTest");
            Directory.CreateDirectory(folder);

            manager.SelectFolder(folder);
            manager.ChangeFileName("task");

            manager.ChangeFileFormat("json");

            Assert.AreEqual("json", manager.FileExtension);
            Assert.IsTrue(File.Exists(manager.FullPath));

            Directory.Delete(folder, true);
        }

        [TestMethod]
        public void Test_09_EditFile()
        {
            var manager = new TestManager("test");
            var fileManager = (IFileManager)manager;

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "PurpleEditTest");
            Directory.CreateDirectory(folder);

            fileManager.SelectFolder(folder);
            fileManager.ChangeFileName("task");

            manager.CreateFile();
            manager.EditFile("HELLO");

            var content = File.ReadAllText(fileManager.FullPath);

            Assert.AreEqual("HELLO", content);

            Directory.Delete(folder, true);
        }

        [TestMethod]
        public void Test_10_ChangeFileExtension()
        {
            var manager = new TestManager("test");
            var fileManager = (IFileManager)manager;

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "PurpleExtTest");
            Directory.CreateDirectory(folder);

            fileManager.SelectFolder(folder);
            fileManager.ChangeFileName("task");

            manager.CreateFile();
            manager.EditFile("DATA");

            manager.ChangeFileExtension("json");

            Assert.AreEqual("json", fileManager.FileExtension);
            Assert.IsTrue(File.Exists(fileManager.FullPath));

            var content = File.ReadAllText(fileManager.FullPath);

            Assert.AreEqual("DATA", content);

            Directory.Delete(folder, true);
        }

        [TestMethod]
        public void Test_11_DeleteFile()
        {
            var manager = new TestManager("test");
            var fileManager = (IFileManager)manager;

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "PurpleDeleteTest");
            Directory.CreateDirectory(folder);

            fileManager.SelectFolder(folder);
            fileManager.ChangeFileName("task");

            manager.CreateFile();

            Assert.IsTrue(File.Exists(fileManager.FullPath));

            var path = fileManager.FullPath;
            manager.DeleteFile();

            Assert.IsFalse(File.Exists(path),
                "File not deleted");

            Directory.Delete(folder, true);
        }
    }
}
