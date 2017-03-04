using ExeAttacher.Core.Exceptions;
using ExeAttacher.Core.Resources;
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

        [Fact]
        public async Task AttachExe_InValidExeFile_ExceptionIsThrown()
        {
            // Arrange.
            string sourceFileName = "test.exe";
            string destinationFileName = "test.att";
            var header = Encoding.UTF8.GetBytes("ZM"); // Wrong header on purpouse.
            var content = new byte[100];
            content.FillWithRandomData();

            var sourceStream = new MemoryStream(header.Concat(content).ToArray());

            this.fileHandlingServiceMock.Setup(fh => fh.FileExists(sourceFileName))
                .Returns(true);
            this.fileHandlingServiceMock.Setup(fh => fh.GetFileStream(sourceFileName))
                .Returns(sourceStream);

            var testee = this.GetAttachService();

            // Act.
            var exceptionAssertion = await Assert.ThrowsAsync<Exception<NoExeFileExceptionArgs>>(() => testee.AttachExe(sourceFileName));

            // Assert.
            this.fileHandlingServiceMock.Verify(fh => fh.FileExists(sourceFileName), Times.Once);
            this.fileHandlingServiceMock.Verify(fh => fh.GetFileStream(sourceFileName), Times.Once);
            this.fileHandlingServiceMock.Verify(fh => fh.GetFileStream(destinationFileName), Times.Once);
            exceptionAssertion.Args.FilePath.Should().Be(sourceFileName);
            exceptionAssertion.Args.Message.Should().Be(string.Format(ErrorMessages.InvalidExeFile, sourceFileName));

            // Cleanup.
            sourceStream.Dispose();
        }

        [Fact]
        public async Task AttachExe_InValidExeFileName_ExceptionIsThrown()
        {
            // Arrange.
            string sourceFileName = "test.wrongExtension";
            
            this.fileHandlingServiceMock.Setup(fh => fh.FileExists(sourceFileName))
                .Returns(true);
            
            var testee = this.GetAttachService();

            // Act.
            var exceptionAssertion = await Assert.ThrowsAsync<Exception<NoExeFileExceptionArgs>>(() => testee.AttachExe(sourceFileName));

            // Assert.
            this.fileHandlingServiceMock.Verify(fh => fh.FileExists(sourceFileName), Times.Once);
            exceptionAssertion.Args.FilePath.Should().Be(sourceFileName);
            exceptionAssertion.Args.Message.Should().Be(string.Format(ErrorMessages.InvalidExeFile, sourceFileName));
        }

        [Fact]
        public async Task AttachExe_FileNotExists_ExceptionIsThrown()
        {
            // Arrange.
            string sourceFileName = "test.exe";

            this.fileHandlingServiceMock.Setup(fh => fh.FileExists(sourceFileName))
                .Returns(false);

            var testee = this.GetAttachService();

            // Act.
            var exceptionAssertion = await Assert.ThrowsAsync<Exception<NoAccessFileExceptionArgs>>(() => testee.AttachExe(sourceFileName));

            // Assert.
            this.fileHandlingServiceMock.Verify(fh => fh.FileExists(sourceFileName), Times.Once);
            exceptionAssertion.Args.FilePath.Should().Be(sourceFileName);
            exceptionAssertion.Args.Message.Should().Be(string.Format(ErrorMessages.CannotAccessToFile, sourceFileName));
        }

        [Fact]
        public async Task RevertExe_InValidExeFileName_ExceptionIsThrown()
        {
            // Arrange.
            string sourceFileName = "test.wrongExtension";

            this.fileHandlingServiceMock.Setup(fh => fh.FileExists(sourceFileName))
                .Returns(true);

            var testee = this.GetAttachService();

            // Act.
            var exceptionAssertion = await Assert.ThrowsAsync<Exception<NoAttachFileExceptionArgs>>(() => testee.RevertExe(sourceFileName));

            // Assert.
            this.fileHandlingServiceMock.Verify(fh => fh.FileExists(sourceFileName), Times.Once);
            exceptionAssertion.Args.FilePath.Should().Be(sourceFileName);
            exceptionAssertion.Args.Message.Should().Be(string.Format(ErrorMessages.InvalidAttachedFile, sourceFileName));
        }

        [Fact]
        public async Task RevertExe_FileNotExists_ExceptionIsThrown()
        {
            // Arrange.
            string sourceFileName = "test.att";

            this.fileHandlingServiceMock.Setup(fh => fh.FileExists(sourceFileName))
                .Returns(false);

            var testee = this.GetAttachService();

            // Act.
            var exceptionAssertion = await Assert.ThrowsAsync<Exception<NoAccessFileExceptionArgs>>(() => testee.RevertExe(sourceFileName));

            // Assert.
            this.fileHandlingServiceMock.Verify(fh => fh.FileExists(sourceFileName), Times.Once);
            exceptionAssertion.Args.FilePath.Should().Be(sourceFileName);
            exceptionAssertion.Args.Message.Should().Be(string.Format(ErrorMessages.CannotAccessToFile, sourceFileName));
        }

        private IAttachService GetAttachService()
        {
            return new AttachService(this.fileHandlingServiceMock.Object);
        }
    }
}