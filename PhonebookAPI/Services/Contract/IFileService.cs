namespace PhonebookAPI.Services.Contract
{
    public interface IFileService
    {
         byte[] ToByteArray(IFormFile file);
    }
}
