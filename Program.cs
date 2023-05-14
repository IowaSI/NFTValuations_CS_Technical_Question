using NFTValuations;
using System.Numerics;
using System.Net.Http;
using NFTValuations.Models;
using System;
using UrlParser = NFTValuations.UrlParser;
using System.Text;

namespace HelloWorld
{
    class Program
    {        
        static void Main(string[] args)
        {
            var contractAddresses = new List<ContractAddressModel>() 
            {
                new ContractAddressModel{ TokenId = 0, Address = "0x1a92f7381b9f03921564a437210bb9396471050c" },//https://api.coolcatsnft.com/cat/0
                new ContractAddressModel{ TokenId = 30, Address =  "0xec9c519d49856fd2f8133a0741b4dbe002ce211b" },//https://ipfs.io/ipfs/QmVtJUJrjWfthrGFvkwFciZcvoNLzCrC6EXAQwimYBUZeQ/30
                new ContractAddressModel{ TokenId = 1, Address ="0xeaa4c58427c184413b04db47889b28b5c98ebb7b" },//ipfs://QmUEs5w1WZKrHkxw3p3fYGj2jZKfLZCURwWvsXJdzHLJB9/1
                new ContractAddressModel{ TokenId = 0, Address = "0x0b22fe0a2995c5389ac093400e52471dca8bb48a" },//ipfs://QmYFXRu7pjj9KEGcMm6u7n8WCH8nAHamethtPrkoQS8zGq/0
                new ContractAddressModel { TokenId = 9896, Address = "0xb4d06d46a8285f4ec79fd294f78a881799d8ced9" },//https://3land.mypinata.cloud/ipfs/QmYZzQ4xkH2Js6bbp3xEFwUURxxLdDG5HkXr3tA8GAdbG2/6031.json
                new ContractAddressModel { TokenId = 285, Address = "0xb668beb1fa440f6cf2da0399f8c28cab993bdd65" },//data:application/json;base64,eyJuYW1lIjogIkNpdGl6ZW4gIzI4NSIsImF0dHJpYnV0ZXMiOiBbeyJ0cmFpdF90eXBlIjogIkNsYXNzIiwgInZhbHVlIjogIkN5YmVyIE5pbmphIn0seyJ0cmFpdF90eXBlIjogIlJhY2UiLCAidmFsdWUiOiAiRGVtb24ifSx7InRyYWl0X3R5cGUiOiAiU3RyZW5ndGgiLCAibWF4X3ZhbHVlIjogMTAwLCAidmFsdWUiOiA5Nn0seyJ0cmFpdF90eXBlIjogIkludGVsbGlnZW5jZSIsICJtYXhfdmFsdWUiOiAxMDAsICJ2YWx1ZSI6IDkwfSx7InRyYWl0X3R5cGUiOiAiQXR0cmFjdGl2ZW5lc3MiLCAibWF4X3ZhbHVlIjogMTAwLCAidmFsdWUiOiA5Nn0seyJ0cmFpdF90eXBlIjogIlRlY2ggU2tpbGwiLCAibWF4X3ZhbHVlIjogMTAwLCAidmFsdWUiOiA5NX0seyJ0cmFpdF90eXBlIjogIkNvb2wiLCAibWF4X3ZhbHVlIjogMTAwLCAidmFsdWUiOiA4NH0seyJ0cmFpdF90eXBlIjogIkV5ZXMiLCAidmFsdWUiOiAiQW5ncnkifSx7InRyYWl0X3R5cGUiOiAiQWJpbGl0eSIsICJ2YWx1ZSI6ICJEZW5zZSJ9LHsidHJhaXRfdHlwZSI6ICJMb2NhdGlvbiIsICJ2YWx1ZSI6ICJBcmllcyBDb21wb3VuZCJ9LHsidHJhaXRfdHlwZSI6ICJBZGRpdGlvbmFsIEl0ZW0iLCAidmFsdWUiOiAiTm9uZSJ9LHsidHJhaXRfdHlwZSI6ICJXZWFwb24iLCAidmFsdWUiOiAiUGlzdG9sIn0seyJ0cmFpdF90eXBlIjogIlZlaGljbGUiLCAidmFsdWUiOiAiTm9uZSJ9LHsidHJhaXRfdHlwZSI6ICJBcHBhcmVsIiwgInZhbHVlIjogIldvcmsgQ2xvdGhlcyAzIn0seyJ0cmFpdF90eXBlIjogIkhlbG0iLCAidmFsdWUiOiAiQnVzaW5lc3MgSGVsbWV0IDEifSx7InRyYWl0X3R5cGUiOiAiUmV3YXJkIFJhdGUiLCAidmFsdWUiOiA3fSx7InRyYWl0X3R5cGUiOiAiR2VuZGVyIiwgInZhbHVlIjogIk1hbGUifV0sImRlc2NyaXB0aW9uIjogIlRoYW5rcyBmb3Igd2hlcmUgeW91IHRvb2sgbWUiLCAiaW1hZ2VfZGF0YSI6ICJkYXRhOmltYWdlL3N2Zyt4bWw7YmFzZTY0LFBITjJaeUI0Yld4dWN6MGlhSFIwY0RvdkwzZDNkeTUzTXk1dmNtY3ZNakF3TUM5emRtY2lJSEJ5WlhObGNuWmxRWE53WldOMFVtRjBhVzg5SW5oTmFXNVpUV2x1SUcxbFpYUWlJSFpwWlhkQ2IzZzlJakFnTUNBeE1qQXdJREV5TURBaVBqeHBiV0ZuWlNCb2NtVm1QU0pvZEhSd2N6b3ZMMjVsYjNSdmEzbHZMbTE1Y0dsdVlYUmhMbU5zYjNWa0wybHdabk12VVcxUVRGYzJkVFZOVW5WME1XSTRhWGxXWXpRM1JWUTFla0ZxT1ZaaFJ6SkhkM2xxWTNWTFRHOWxkRmR6VkM5aVlXTnJaM0p2ZFc1a0x6WXVjRzVuSWk4K1BHbHRZV2RsSUdoeVpXWTlJbWgwZEhCek9pOHZibVZ2ZEc5cmVXOHViWGx3YVc1aGRHRXVZMnh2ZFdRdmFYQm1jeTlSYlZCTVZ6WjFOVTFTZFhReFlqaHBlVlpqTkRkRlZEVjZRV281Vm1GSE1rZDNlV3BqZFV0TWIyVjBWM05VTDJKdlpIa3ZOaTV3Ym1jaUx6NDhhVzFoWjJVZ2FISmxaajBpYUhSMGNITTZMeTl1Wlc5MGIydDVieTV0ZVhCcGJtRjBZUzVqYkc5MVpDOXBjR1p6TDFGdFVFeFhOblUxVFZKMWRERmlPR2w1Vm1NME4wVlVOWHBCYWpsV1lVY3lSM2Q1YW1OMVMweHZaWFJYYzFRdlkyeHZkR2d2TWk1d2JtY2lMejQ4YVcxaFoyVWdhSEpsWmowaWFIUjBjSE02THk5dVpXOTBiMnQ1Ynk1dGVYQnBibUYwWVM1amJHOTFaQzlwY0daekwxRnRVRXhYTm5VMVRWSjFkREZpT0dsNVZtTTBOMFZVTlhwQmFqbFdZVWN5UjNkNWFtTjFTMHh2WlhSWGMxUXZhR0Z1WkM5bmRXNHZOaTV3Ym1jaUx6NDhhVzFoWjJVZ2FISmxaajBpYUhSMGNITTZMeTl1Wlc5MGIydDVieTV0ZVhCcGJtRjBZUzVqYkc5MVpDOXBjR1p6TDFGdFVFeFhOblUxVFZKMWRERmlPR2w1Vm1NME4wVlVOWHBCYWpsV1lVY3lSM2Q1YW1OMVMweHZaWFJYYzFRdmQyVmhjRzl1THpZdWNHNW5JaTgrUEdsdFlXZGxJR2h5WldZOUltaDBkSEJ6T2k4dmJtVnZkRzlyZVc4dWJYbHdhVzVoZEdFdVkyeHZkV1F2YVhCbWN5OVJiVkJNVnpaMU5VMVNkWFF4WWpocGVWWmpORGRGVkRWNlFXbzVWbUZITWtkM2VXcGpkVXRNYjJWMFYzTlVMMmhsWVdRdk5pNXdibWNpTHo0OGFXMWhaMlVnYUhKbFpqMGlhSFIwY0hNNkx5OXVaVzkwYjJ0NWJ5NXRlWEJwYm1GMFlTNWpiRzkxWkM5cGNHWnpMMUZ0VUV4WE5uVTFUVkoxZERGaU9HbDVWbU0wTjBWVU5YcEJhamxXWVVjeVIzZDVhbU4xUzB4dlpYUlhjMVF2YUdWc2JTODJMbkJ1WnlJdlBqd3ZjM1puUGc9PSJ9
                new ContractAddressModel { TokenId = 1234, Address = "0xbc4ca0eda7647a8ab7c2061c2e118a18a936f13d" },//ipfs://QmeSjSinHpPnmXmspMjwiXyN6zS4E9zccariGR3jxcaWtq/1234
                new ContractAddressModel { TokenId = 287, Address = "0xdbb163b22e839a26d2a5011841cb4430019020f9" },
                new ContractAddressModel { TokenId = 287, Address = "0xdbb163b22e839a26d2a5011841cb4430019020f9" },
                new ContractAddressModel { TokenId = 30000000, Address = "0x1cb1a5e65610aeff2551a50f76a87a7d3fb649c6" },
                new ContractAddressModel { TokenId = 18, Address = "0x845dd2a7ee2a92a0518ab2135365ed63fdba0c88" },
                new ContractAddressModel { TokenId = 4563, Address = "0x7bd29408f11d2bfc23c34f18275bbf23bb716bc7" },
                new ContractAddressModel { TokenId = 2000000, Address = "0x059edd72cd353df5106d2b9cc5ab83a52287ac3a" },
                new ContractAddressModel { TokenId = 1000001, Address = "0x1b829b926a14634d36625e60165c0770c09d02b2" },
                new ContractAddressModel { TokenId = 1, Address = "0xd4e4078ca3495de5b1d4db434bebc5a986197782" },
                new ContractAddressModel { TokenId = 0, Address = "0x892848074ddea461a15f337250da3ce55580ca85" }
            };
            var extractor = new NFTMetadataExtractor();

            foreach (var address in contractAddresses)
            {
                Console.WriteLine("----------------------------------------------------------------------");
                try
                {                    
                    var metadata = extractor.ExtractMetadata(address.Address, address.TokenId).Result;                    
                    
                    Console.WriteLine("address.Address, address.TokenId:");
                    Console.WriteLine($"{address.Address},{address.TokenId}");
                    Console.WriteLine("NFT Metadata:");
                    Console.WriteLine(metadata);

                    var metadataContent = UrlParser.Parse(metadata);

                    Console.WriteLine("NFT metadataContent:");
                    Console.WriteLine(metadataContent);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("*************ERROR*************");
                    Console.WriteLine($"Error:{ex.Message} for address: {address.Address}, tokenId: {address.TokenId}, innerException: {ex.InnerException?.Message}");
                }
            }
            Console.ReadKey();
        }
    }
}