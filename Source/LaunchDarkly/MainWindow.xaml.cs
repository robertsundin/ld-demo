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
		private static readonly LdClient _client = new LdClient("sdk-1898dd97-4c2e-4ee9-bbac-80448878f407");

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

			return User.WithKey(user)
				.AndFirstName("My")
				.AndLastName("Name")
				.AndEmail("unknown@thomascook.se")
				.AndCustomAttribute("auth_level", GetRoles(user));
		}

		private List<string> GetRoles(string user)
		{
			if(user == "robban")
				return new List<string>{ "developer", "user" };

			return new List<string>{ "user" };
		}
	}
}
