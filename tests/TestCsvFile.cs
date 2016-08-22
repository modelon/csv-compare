using System.IO;
using NUnit.Framework;
using CsvCompare;
using CurveCompare;

namespace tests
{
    /// <summary>
    /// Test CsvFile class
    /// </summary>
    [TestFixture]
    public class TestCsvFile
    {
        const string FileName = "some.file";

        /// <summary>
        /// Test creating CsvFile object from non-exsting file.
        /// </summary>
        [Test]
        public void ConstructorFileNotFound()
        {
            // make sure the file does not exists
            Assert.IsFalse(File.Exists(FileName));

            // check that we get appropriate exception
            Assert.That(() => new CsvFile(FileName,
                                          new CsvCompare.Options(),
                                          new CsvCompare.Log()),
                        Throws.TypeOf<FileNotFoundException>());
        }
    }

    /// <summary>
    /// Test generation of errors curve used in result reports
    /// </summary>
    [TestFixture]
    public class TestCreateErrorsCurve
    {
        /// <summary>
        /// test the case where there is one error detected 'in the middle'
        /// </summary>
        [Test]
        public void OneError()
        {
            var compare = new Curve("compare",
                                    new double[] { 0.0, 2.0, 4.0 },
                                    new double[] { 0.0, 0.0, 0.0 });

            var error = new Curve("error",
                                  new double[] { 2.0 },
                                  new double[] { 0.5 });

            var res = CsvFile.CreateErrorsCurve(compare, error, true);

            Assert.That(res.X, Is.EquivalentTo(compare.X));
            Assert.That(res.Y, Is.EquivalentTo(new double[] { 0.0, 0.5, 0.0 }));
        }

        /// <summary>
        /// test the case where there is an error at the last point on the compare curve
        /// </summary>
        [Test]
        public void ErrorAtEnd()
        {
            var compare = new Curve("compare",
                                    new double[] { 0.0, 1.0, 2.0, 3.0 },
                                    new double[] { 0.1, 0.2, 0.3, 0.4 });

            var error = new Curve("error",
                                  new double[] { 2.0, 3.0 },
                                  new double[] { 0.1, 0.2 });

            var res = CsvFile.CreateErrorsCurve(compare, error, true);

            Assert.That(res.X, Is.EquivalentTo(compare.X));
            Assert.That(res.Y, Is.EquivalentTo(new double[] { 0.0, 0.0, 0.1, 0.2 }));
        }
    }
}
