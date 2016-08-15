using NUnit.Framework;
using CsvCompare;
using System;
using System.IO;

namespace tests
{
    /// <summary>
    /// This is a suite of tests where we invoke the csv-compare tool with
    /// two csv files, result and reference, and expect csv-compare to report that
    /// curves in csv files are within the reference curve.
    ///
    /// The goal of these tests is to check that csv-compare handles correctly positive
    /// tests.
    /// </summary>
    [TestFixture]
    class TestCompareCsvFiles
    {
        string reportDir;

        private static string MakeTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        [SetUp]
        protected void SetUp()
        {
            reportDir = MakeTemporaryDirectory();
        }

        [TearDown]
        protected void TearDown()
        {
            Directory.Delete(reportDir, true);
        }

        /// <summary>
        /// Utility functions that invokes csv-compare tool on a pair of csv files and checks
        /// that no errors are reported (exit code is 0).
        /// </summary>
        /// <param name="name">name of the curve to check</param>
        protected void CompareCsvFiles(string name)
        {
            string csvFilesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "csvfiles");

            string baseCsv = Path.Combine(csvFilesDir, name + ".csv");
            string refCsv = Path.Combine(csvFilesDir, name + "_ref.csv");

            var exitCode = Program.Main(new[] { "--reportdir", reportDir, baseCsv, refCsv});

            /* TODO: we should probably do some sanity check of the contents of report dir */
            Assert.That(exitCode, Is.EqualTo(0));
        }

        /// <summary>
        /// Test on tube generation at the end-points of the curve.
        /// Check that curves with high slope at the end-point are handled.
        /// </summary>
        [Test]
        public void CompareEndPoint()
        {
            CompareCsvFiles("endpoint");
        }

        ///
        /// 'peak' and 'verthor' tests checks that reference tube is not to tight
        /// on curves where the slop changes sign.
        ///
        [Test]
        public void ComparePeak()
        {
            CompareCsvFiles("peak");
        }

        [Test]
        public void CompareVertHor()
        {
            CompareCsvFiles("verthor");
        }
    }
}
