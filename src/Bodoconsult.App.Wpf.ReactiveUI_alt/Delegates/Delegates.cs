namespace Bodoconsult.App.Wpf.ReactiveUI.Delegates;
///// <summary>
///// Delegate used to say the <see cref="LoginWindow">LoginWindow</see> where to go after successful login
///// </summary>
//public delegate void LoginOnSuccess();

/// <summary>
/// Delegate used to verify the given credentials
/// </summary>
/// <param name="userName">User name</param>
/// <param name="password">password</param>
/// <returns>true if login was succesful, else false</returns>
public delegate bool LoginCheckLoginDataDelegate(string userName, string password);

/// <summary>
/// Delegate used to change the password of the user
/// </summary>
/// <param name="oldPassword">old password</param>
/// <param name="newPassword">new password</param>
/// <returns>true if the password was changed, false if not</returns>
/// <returns>true if the password was changed, false if not</returns>
public delegate bool LoginChangePasswordDataDelegate(string oldPassword, string newPassword);

/// <summary>
/// Navigation go-back delegate
/// </summary>
/// <typeparam name="TRegion">Region</typeparam>
/// <typeparam name="TEvent">Event</typeparam>
/// <typeparam name="TInputData">Input data</typeparam>
/// <param name="inputdata"></param>
public delegate void NavigateGoBackDelegate<TRegion, TEvent, TInputData>(TInputData inputdata);