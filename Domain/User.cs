﻿using Dtat.Wallet.Abstractions.SeedWork;

namespace Domain;

public class User : Seedwork.Entity, Dtat.Wallet.Abstractions.IUser<long>
{
	#region Constructor
	public User(string cellPhoneNumber, string displayName) : base()
	{
		DisplayName = displayName;
		UpdateDateTime = InsertDateTime;
		CellPhoneNumber = cellPhoneNumber;

		UserWallets =
			new System.Collections.Generic.List<UserWallet>();

		Transactions =
			new System.Collections.Generic.List<Transaction>();
	}
	#endregion /Constructor

	#region Properties

	#region IsActive
	public bool IsActive { get; set; }
	#endregion /IsActive

	#region UpdateDateTime
	public System.DateTime UpdateDateTime { get; private set; }
	#endregion /UpdateDateTime



	#region DisplayName
	[System.ComponentModel.DataAnnotations.Required
		(AllowEmptyStrings = false)]

	[System.ComponentModel.DataAnnotations.MaxLength
		(length: Constant.MaxLength.DisplayName)]
	public string DisplayName { get; set; }
	#endregion /DisplayName

	#region CellPhoneNumber
	[System.ComponentModel.DataAnnotations.Required
		(AllowEmptyStrings = false)]

	[System.ComponentModel.DataAnnotations.MaxLength
		(length: Constant.MaxLength.CellPhoneNumber)]
	public string CellPhoneNumber { get; set; }
	#endregion /CellPhoneNumber



	#region Hash
	[System.ComponentModel.DataAnnotations.MaxLength
		(length: Constant.MaxLength.Hash)]
	public string? Hash { get; private set; }
	#endregion /Hash

	#region Description
	[System.ComponentModel.DataAnnotations.MaxLength
		(length: Constant.MaxLength.Description)]
	public string? Description { get; set; }
	#endregion /Description

	#region EmailAddress
	[System.ComponentModel.DataAnnotations.MaxLength
		(length: Constant.MaxLength.EmailAddress)]
	public string? EmailAddress { get; set; }
	#endregion /EmailAddress

	#region NationalCode
	[System.ComponentModel.DataAnnotations.MaxLength
		(length: Constant.MaxLength.NationalCode)]
	public string? NationalCode { get; set; }
	#endregion /NationalCode



	#region UserWallets
	[System.Text.Json.Serialization.JsonIgnore]
	public virtual System.Collections.Generic.IList<UserWallet> UserWallets { get; private set; }
	#endregion /UserWallets

	#region Transactions
	[System.Text.Json.Serialization.JsonIgnore]
	public virtual System.Collections.Generic.IList<Transaction> Transactions { get; private set; }
	#endregion /Transactions

	#endregion /Properties

	#region Methods
	public string GetHash()
	{
		var stringBuilder =
			new System.Text.StringBuilder();

		stringBuilder.Append($"{nameof(InsertDateTime)}:{InsertDateTime}");
		stringBuilder.Append('|');
		stringBuilder.Append($"{nameof(IsActive)}:{IsActive}");
		stringBuilder.Append('|');
		stringBuilder.Append($"{nameof(UpdateDateTime)}:{UpdateDateTime}");
		stringBuilder.Append('|');
		stringBuilder.Append($"{nameof(DisplayName)}:{DisplayName}");
		stringBuilder.Append('|');
		stringBuilder.Append($"{nameof(CellPhoneNumber)}:{CellPhoneNumber}");
		stringBuilder.Append('|');
		stringBuilder.Append($"{nameof(Description)}:{Description}");
		stringBuilder.Append('|');
		stringBuilder.Append($"{nameof(EmailAddress)}:{EmailAddress}");
		stringBuilder.Append('|');
		stringBuilder.Append($"{nameof(NationalCode)}:{NationalCode}");

		var text =
			stringBuilder.ToString();

		string result =
			Dtat.Utility.GetSha256(text: text);

		return result;
	}

	public void UpdateHash()
	{
		Hash = GetHash();
	}

	public bool CheckHashValidation()
	{
		var result = GetHash();

		if (string.Compare(result, Hash, ignoreCase: true) == 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	#endregion /Methods
}
