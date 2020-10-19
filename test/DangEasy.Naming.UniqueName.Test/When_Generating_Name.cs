using Xunit;

namespace DangEasy.Naming.UniqueName.Test
{
    public class When_Generating_Name
    {
        [Fact]
        public void TestMany()
        {
            var names = new[]
            {
                "Alpha (2)",
                "Test",
                "Test (1)",
                "Test (2)",
                "Test (1) (1)"
            };

            //fixme - this will yield "Test (2)" which is already in use
            var res = NameGenerator.GetUniqueName(names, "Test");

            Assert.Equal("Test (3)", res);
        }

        [Fact]
        public void Empty_List_Causes_Unchanged_Name()
        {
            var names = new string [] { };

            var res = NameGenerator.GetUniqueName(names, "Test");

            Assert.Equal("Test", res);
        }



        [Fact]
        public void Matched_Name_Is_Suffixed()
        {
            var names = new[]
            {
                "Test"
            };

            var res = NameGenerator.GetUniqueName(names, "Test");

            Assert.Equal("Test (1)", res);
        }


        [Fact]
        public void Suffixed_Name_Is_Incremented()
        {
            var names = new[]
            {
               "Test",
               "Test (1)"
            };

            var res = NameGenerator.GetUniqueName(names, "Test (1)");

            Assert.Equal("Test (2)", res);
        }


        [Fact]
        public void MultiSuffixed_Name_Is_Icremented()
        {
            var names = new[]
            {
                "Test (1)",
               "Test (1) (1)",
            };

            // in this case the "real" name is "Test (1)"
            var res = NameGenerator.GetUniqueName(names, "Test (1) (1)");

            Assert.Equal("Test (1) (2)", res);
        }


        [Fact]
        public void Extra_MultiSuffixed_Name_Is_Ignored()
        {
            var names = new[]
            {
                "Test" ,
                "Test (1)" ,
                "Test (1) (1)",
            };

            var res = NameGenerator.GetUniqueName(names, "Test");

            Assert.Equal("Test (2)", res);
        }


        [Fact]
        public void Suffixed_Name_Causes_Secondary_Suffix()
        {
            var names = new[]
            {
                 "Test (1)"
            };
            var res = NameGenerator.GetUniqueName(names, "Test (1)");

            Assert.Equal("Test (1) (1)", res);
        }
    }
}
