﻿using Caliburn.Micro;
using IF.Lastfm.Core.Api;
using Last.fm_Scrubbler_WPF.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Last.fm_Scrubbler_WPF.ViewModels
{
	/// <summary>
	/// ViewModel for the <see cref="Views.LoginView"/>
	/// </summary>
	class LoginViewModel : PropertyChangedBase
	{
		#region Properties

		/// <summary>
		/// Gets if certain controls should be enabled.
		/// </summary>
		public bool EnableControls
		{
			get { return _enableControls; }
			private set
			{
				_enableControls = value;
				NotifyOfPropertyChange(() => EnableControls);
			}
		}
		private bool _enableControls;

		#endregion Properties

		#region Private Member

		/// <summary>
		/// The Last.fm client.
		/// </summary>
		private LastfmClient _client;

		#endregion Private Member

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="client">Client on which to perform the login.</param>
		public LoginViewModel(LastfmClient client)
		{
			_client = client;
			EnableControls = true;
		}

		/// <summary>
		/// Tries to log the user in with the given credentials.
		/// </summary>
		/// <param name="win">The calling <see cref="Views.LoginView"/>.</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The <see cref="PasswordBox"/> containing the password.</param>
		/// <param name="rememberLoginInformation">Bool determining if the login should be saved.</param>
		public async void Login(Window win, string username, PasswordBox password, bool rememberLoginInformation)
		{
			EnableControls = false;

			var response = await _client.Auth.GetSessionTokenAsync(username, password.Password);

			if(_client.Auth.Authenticated)
			{
				MessageBox.Show("Successfully logged in and authenticated!");

				if (rememberLoginInformation)
					SaveSession();
				else
				{
					Settings.Default.Token = "";
					Settings.Default.Username = "";
					Settings.Default.IsSubscriber = false;
					Settings.Default.Save();
				}

				win.Close();
			}
			else
			{
				MessageBox.Show("Failed to log in or authenticate!");
				EnableControls = true;
			}
		}

		/// <summary>
		/// Saves the user session.
		/// </summary>
		private void SaveSession()
		{
			Settings.Default.Token = _client.Auth.UserSession.Token;
			Settings.Default.Username = _client.Auth.UserSession.Username;
			Settings.Default.IsSubscriber = _client.Auth.UserSession.IsSubscriber;
			Settings.Default.Save();
		}
	}
}