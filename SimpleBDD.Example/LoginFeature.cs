﻿using NUnit.Framework;

namespace SimpleBDD.Example
{
	[TestFixture]
	public partial class LoginFeature
	{
		[Test]
		public void Successful_login()
		{
			_bddRunner.RunScenario(

				Given_user_is_about_to_login,
				Given_user_entered_valid_login,
				Given_user_entered_valid_password,
				When_user_clicked_login_button,
				Then_login_is_successful,
				Then_welcome_message_is_returned_containing_user_name);
		}

		[Test]
		public void Wrong_login_provided_causes_login_to_fail()
		{
			_bddRunner.RunScenario(

				Given_user_is_about_to_login,
				Given_user_entered_invalid_login,
				Given_user_entered_valid_password,
				When_user_clicked_login_button,
				Then_login_is_unsuccessful,
				Then_invalid_login_or_password_error_message_is_returned);
		}

		[Test]
		public void Wrong_password_provided_causes_login_to_fail()
		{
			_bddRunner.RunScenario(

				Given_user_is_about_to_login,
				Given_user_entered_valid_login,
				Given_user_entered_invalid_password,
				When_user_clicked_login_button,
				Then_login_is_unsuccessful,
				Then_invalid_login_or_password_error_message_is_returned);
		}
	}
}
