using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiREST
{
	public class MainViewModel
	{
		HttpClient client;
		JsonSerializerOptions _serializerOptions;
		string baseURL = "https://66742ef375872d0e0a9570f6.mockapi.io";
		private List<User> Users;
		public MainViewModel() { 
			client = new HttpClient();
			_serializerOptions= new JsonSerializerOptions
			{
				WriteIndented = true,
			};
		}
		public ICommand GetAllUserCommand => new Command(async () =>
		{
			var url = $"{baseURL}/users";
			var response = await client.GetAsync(url);
			if(response.IsSuccessStatusCode)
			{
				//var content = await response.Content.ReadAsStringAsync();
				using(var responseStream = await response.Content.ReadAsStreamAsync())
				{
					var data =
					await JsonSerializer.DeserializeAsync<List<User>>(responseStream, _serializerOptions);
					Users = data;
				}
			}
		});
		public ICommand GetSingleUserCommand  => new Command(async () =>
		{
			var url = $"{baseURL}/users/25";
			var response = await client.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
			}
		});
		public ICommand AddUserCommand => new Command(async () =>
		{
			var url = $"{baseURL}/users";
			var user = new User
			{
				createdAt = DateTime.Now,
				name = "Bhat",
				avatar = "https://fakeimg.pl/350x200/?text=MAUT"
			};
			string json = JsonSerializer.Serialize<User>(user,_serializerOptions);

			StringContent content = new StringContent(json,Encoding.UTF8,"application/json");
			var response = await client.PostAsync(url, content);

		});
		public ICommand UpdateUserCommand => new Command(async () =>
		{
			var user = Users.FirstOrDefault(x => x.id == "1");
			var url = $"{baseURL}/users/1";
			user.name = "Bhupendra Bhat";
			
			string json = JsonSerializer.Serialize<User>(user, _serializerOptions);

			StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await client.PutAsync(url, content);

		});

		public ICommand DeleteUserCommand => new Command(async () =>
		{
			var url = $"{baseURL}/users/10";
			
			var response = await client.DeleteAsync(url);

		});

	}
}
