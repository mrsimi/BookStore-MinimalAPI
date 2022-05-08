using System.Text;
using System.Text.Json;

namespace bookstoreapi
{
    public static class BookStoreService
    {

        static string firebaseDatabaseUrl = "https://crud-bookstore-default-rtdb.firebaseio.com/";
        static string firebaseDatabaseDocument = "BookStore";
        static readonly HttpClient client = new HttpClient();
        public static async Task<Book> Add(Book book)
        {
            book.Id = Guid.NewGuid().ToString("N");
            string bookJsonString = JsonSerializer.Serialize(book);

            var payload = new StringContent(bookJsonString, Encoding.UTF8, "application/json");

            string url = $"{firebaseDatabaseUrl}" +
                        $"{firebaseDatabaseDocument}/" +
                        $"{book.Id}.json";


            var httpResponseMessage = await client.PutAsync(url, payload);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<Book>(contentStream);
                return result;
            }

            return null;

        }

        public static async Task<Book> GetById(string id)
        {
            string url = $"{firebaseDatabaseUrl}" +
                        $"{firebaseDatabaseDocument}/" +
                        $"{id}.json";

            var httpResponseMessage = await client.GetAsync(url);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                if (contentStream != null && contentStream != "null")
                {
                    var result = JsonSerializer.Deserialize<Book>(contentStream);

                    return result;
                }               
            }

            return null;

        }

        public static async Task<List<Book>> GetAll()
        {
            string url = $"{firebaseDatabaseUrl}" +
                        $"{firebaseDatabaseDocument}.json";

            var httpResponseMessage = await client.GetAsync(url);
            List<Book> entries = new List<Book>();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                if (contentStream != null && contentStream != "null")
                {
                    var result = JsonSerializer.Deserialize<Dictionary<string, Book>>(contentStream);

                    entries = result.Select(x => x.Value).ToList();
                }
            }

            return entries;

        }


        public static async Task<string> DeleteById(string id)
        {
            string url = $"{firebaseDatabaseUrl}" +
                        $"{firebaseDatabaseDocument}/" +
                        $"{id}.json";

            var httpResponseMessage = await client.DeleteAsync(url);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
                if(contentStream == "null")
                {
                    return "Deleted";
                }
            }

            return null;
        }

        public static async Task<Book> Update(Book book, string id)
        {
            book.Id = id;
            string bookJsonString = JsonSerializer.Serialize(book);

            var payload = new StringContent(bookJsonString, Encoding.UTF8, "application/json");

            string url = $"{firebaseDatabaseUrl}" +
                        $"{firebaseDatabaseDocument}/" +
                        $"{id}.json";


            var httpResponseMessage = await client.PutAsync(url, payload);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();

                if (contentStream != null && contentStream != "null")
                {
                    var result = JsonSerializer.Deserialize<Book>(contentStream);

                    return result;
                }         
            }

            return null;

        }
    }

    public class Book
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string AuthorName { get; set; }
        public string Year { get; set; }
    }
}