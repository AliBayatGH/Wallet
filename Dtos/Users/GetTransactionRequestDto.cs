﻿namespace Dtos.Users;

public class GetTransactionRequestDto : object
{
	#region Constructor
	public GetTransactionRequestDto() : base()
	{
		User = new();
	}
	#endregion /Constructor

	#region Properties

	#region User
	/// <summary>
	/// اطلاعات کاربر
	/// </summary>
	[System.ComponentModel.DataAnnotations.Required]
	public GetTransactionRequestUserDto User { get; set; }
	#endregion /User

	#region TransactionId
	public long TransactionId { get; set; }
	#endregion /TransactionId

	#region WalletToken
	/// <summary>
	/// توکن کیف پول
	/// </summary>
	[System.ComponentModel.DataAnnotations.Required]
	public System.Guid WalletToken { get; set; }
	#endregion /WalletToken

	#region CompanyToken
	/// <summary>
	/// توکن شرکت
	/// </summary>
	[System.ComponentModel.DataAnnotations.Required]
	public System.Guid CompanyToken { get; set; }
	#endregion /CompanyToken

	#endregion /Properties
}
