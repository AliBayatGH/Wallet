﻿namespace Domain
{
	public class UserWallet : Seedwork.Entity, Dtat.Wallet.Abstractions.IUserWallet<long>
	{
		#region Constructor
		public UserWallet(long userId, long walletId, string companyUserIdentity)
		{
			UserId = userId;
			WalletId = walletId;
			CompanyUserIdentity = companyUserIdentity;

			UpdateDateTime = InsertDateTime;
		}
		#endregion /Constructor

		#region Properties

		public long UserId { get; private set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public virtual User? User { get; private set; }



		public long WalletId { get; private set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public virtual Wallet? Wallet { get; private set; }



		public bool IsActive { get; set; }

		public System.DateTime UpdateDateTime { get; private set; }



		public decimal Balance { get; set; }



		public string CompanyUserIdentity { get; set; }

		public bool PaymentFeatureIsEnabled { get; set; }

		public bool DepositeFeatureIsEnabled { get; set; }

		public bool WithdrawFeatureIsEnabled { get; set; }

		public bool TransferFeatureIsEnabled { get; set; }



		public string? Hash { get; set; }

		public string? Description { get; set; }

		#endregion /Properties
	}
}
