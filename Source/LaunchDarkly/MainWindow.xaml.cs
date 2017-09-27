using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LaunchDarkly.Client;
using Newtonsoft.Json.Linq;

namespace LaunchDarkly
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private static readonly LdClient _client = new LdClient("sdk-fbcf8b2f-49a6-4e0b-a13a-d0be20282823");

		public MainWindow()
		{
			InitializeComponent();
		}

		private void go_Click(object sender, RoutedEventArgs e)
		{
			var user = GetUser();
			
			// BOOL
			onOrOff.Content = _client.BoolVariation("on-or-off", user, false);

			// INT
			someNumber.Content = _client.IntVariation("some-number", user, 999);

			// JSON
			var value = _client.JsonVariation("current-environment", user, "NONE");
			var environment = value["Environment"].Value<string>();

			currentEnvironment.Content = environment;
		}

		private User GetUser()
		{
			var user = currentUser.Text;

			if(string.IsNullOrEmpty(user))
			{
				return User.WithKey("xxx").AndAnonymous(true);
			}

			return GetFromAuth(user);
		}

		private User GetFromAuth(string user)
		{
			using(var authReader = new AuthReader.AuthReaderSoapClient("AuthReaderSoap"))
			{
				var adUser = authReader.GetActiveDirectoryUserInformation(user, null);

				return User.WithKey(user)
					.AndFirstName(adUser.User.FirstName)
					.AndLastName(adUser.User.LastName)
					.AndEmail(adUser.User.Email)
					.AndCustomAttribute("auth_level", GetRoles(user));
			}
		}

		private List<string> GetRoles(string user)
		{
			if(user == "1535")
				return new List<string>{ "developer", "user" };

			return new List<string>{ "user" };
		}
	}
}
