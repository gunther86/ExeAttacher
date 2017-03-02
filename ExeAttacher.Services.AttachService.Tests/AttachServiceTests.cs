using ExeAttacher.Core.Services;
using FluentAssertions;
using Moq;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExeAttacher.Services.AttachService.Tests
{
    public class AttachServiceTests
    {
        private readonly Mock<IFileHandlingService> fileHandlingServiceMock;

        public AttachServiceTests()
        {
            this.fileHandlingServiceMock = new Mock<IFileHandlingService>();
        }

        [Fact]
        public async Task AttachExe_ValidExeFile_HeaderIsRemoved()
        {
            // Arrange.
            string sourceFileName = "test.exe";
            string destinationFileName = "test.att";
            var header = Encoding.UTF8.GetBytes("MZ");
            var content = new byte[100];
            content.FillWithRandomData();

            var sourceStream = new MemoryStream(header.Concat(content).ToArray());
            var destinationStream = new MemoryStream();

            this.fileHandlingServiceMock.Setup(fh => fh.FileExists(sourceFileName))
                .Returns(true);
            this.fileHandlingServiceMock.Setup(fh => fh.GetFileStream(sourceFileName))
                .Returns(sourceStream);
            this.fileHandlingServiceMock.Setup(fh => fh.GetFileStream(destinationFileName))
                .Returns(destinationStream);

            var testee = this.GetAttachService();

            // Act.
            await testee.AttachExe(sourceFileName);

            // Assert.
            this.fileHandlingServiceMock.Verify(fh => fh.FileExists(sourceFileName), Times.Once);
            this.fileHandlingServiceMock.Verify(fh => fh.GetFileStream(sourceFileName), Times.Once);
            this.fileHandlingServiceMock.Verify(fh => fh.GetFileStream(destinationFileName), Times.Once);
            destinationStream.ToArray().Should().BeEquivalentTo(content);

            // Cleanup.
            sourceStream.Dispose();
            destinationStream.Dispose();
        }

        [Fact]
        public void AttachExe_ValidExeFile_ExtensionIsChanged()
        {

        }

        private IAttachService GetAttachService()
        {
            return new AttachService(this.fileHandlingServiceMock.Object);
        }
    }
}