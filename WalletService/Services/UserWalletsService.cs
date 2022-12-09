﻿using Infrastructure;
using System.Linq;

namespace Server.Services;

public static class UserWalletsService : object
{
	static UserWalletsService()
	{
	}

	#region CreateOrUpdateUserWallet()
	public static Domain.UserWallet
		CreateOrUpdateUserWallet
		(Data.DatabaseContext databaseContext,
		long userId, long walletId, string? additionalData)
	{
		var userWallet =
			databaseContext.UserWallets
			.Where(current => current.UserId == userId)
			.Where(current => current.WalletId == walletId)
			.FirstOrDefault();

		if (userWallet == null)
		{
			userWallet =
				new Domain.UserWallet(userId: userId, walletId: walletId)
				{
					IsActive = true,

					AdditionalData = additionalData,
				};

			databaseContext.Add(entity: userWallet);
		}
		else
		{
			userWallet.AdditionalData = additionalData;
		}

		userWallet.UpdateHash();

		// دستور ذیل باید نوشته شود Id به دلیل نوع
		databaseContext.SaveChanges();

		return userWallet;
	}
	#endregion /CreateOrUpdateUserWallet()

	#region CheckAndGetUserWallet()
	public static Dtat.Result<Domain.UserWallet> CheckAndGetUserWallet
		(Data.DatabaseContext databaseContext, string cellPhoneNumber, System.Guid walletToken)
	{
		var result =
			new Dtat.Result<Domain.UserWallet>();

		var userWallet =
			databaseContext.UserWallets
			.Where(current => current.Wallet != null && current.Wallet.Token == walletToken)
			.Where(current => current.User != null && current.User.CellPhoneNumber == cellPhoneNumber)
			.FirstOrDefault();

		if (userWallet == null)
		{
			var errorMessage =
				Resources.Messages.Errors.TheUserDoesNotHaveAccessToThisWallet;

			result.AddErrorMessages
				(message: errorMessage);

			return result;
		}

		var hashValidation =
			userWallet.CheckHashValidation();

		if (hashValidation == false)
		{
			var errorMessage =
				Resources.Messages.Errors.InconsitencyDataForUserWallet;

			result.AddErrorMessages
				(message: errorMessage);

			return result;
		}

		if (userWallet.IsActive == false)
		{
			var errorMessage =
				Resources.Messages.Errors.TheUserAccessToThisWalletIsNotActive;

			result.AddErrorMessages
				(message: errorMessage);

			return result;
		}

		result.Data = userWallet;

		return result;
	}
	#endregion /CheckAndGetUserWallet()

	#region GetUserBalanceWithCheckingDataConsistency()
	public static Dtat.Result<decimal> GetUserBalanceWithCheckingDataConsistency
		(Data.DatabaseContext databaseContext, System.Guid walletToken,
		string cellPhoneNumber, Domain.UserWallet userWallet)
	{
		var result =
			new Dtat.Result<decimal>();

		// **************************************************
		// نوع اول
		// بررسی معتبر بودن مقدار مانده کاربر
		// در کیف پول بر اساس هش موجود در رکورد
		// **************************************************
		// نکته مهم: نیازی به دستورات ذیل نیست، این دستورات
		// در دستورات قبلی چک می‌شود
		// **************************************************
		//var hashValidation =
		//	userWallet.CheckHashValidation();

		//if (hashValidation == false)
		//{
		//	var errorMessage =
		//		$"There is inconsitency data (Type 1) in user wallet balance!";

		//	result.AddErrorMessages(message: errorMessage);

		//	return result;
		//}
		// **************************************************

		// **************************************************
		// نوع دوم
		// بررسی معتبر بودن مقدار مانده کاربر در کیف پول
		// بر اساس کلیه تراکنش‌های کاربر در کیف پول مربوطه
		// **************************************************
		var balance =
			databaseContext.Transactions
			.Where(current => current.Wallet != null && current.Wallet.Token == walletToken)
			.Where(current => current.User != null && current.User.CellPhoneNumber == cellPhoneNumber)
			.Sum(current => current.Amount);

		if (balance != userWallet.Balance)
		{
			var errorMessage =
				Resources.Messages.Errors.InconsitencyDataForUserBalance;

			result.AddErrorMessages(message: errorMessage);

			return result;
		}
		// **************************************************

		result.Data =
			userWallet.Balance;

		return result;
	}
	#endregion /GetUserBalanceWithCheckingDataConsistency()

	#region GetUserWithdrawBalance()
	public static Dtat.Result<decimal> GetUserWithdrawBalance
		(Data.DatabaseContext databaseContext,
		System.Guid walletToken, string cellPhoneNumber, IUtility utility)
	{
		var result =
			new Dtat.Result<decimal>();

		var now =
			utility.GetNow();

		if (now != now.Date)
		{
			now =
				now.Date.AddDays(value: 1);
		}

		// همه خروج و برداشت مبالغ
		var allAntiDeposite =
			databaseContext.Transactions
			.Where(current => current.Amount < 0)
			.Where(current => current.Wallet != null && current.Wallet.Token == walletToken)
			.Where(current => current.User != null && current.User.CellPhoneNumber == cellPhoneNumber)
			.Sum(current => current.Amount);

		// همه مبالغ قابل برداشت
		var allDeposite =
			databaseContext.Transactions
			.Where(current => current.Amount > 0)
			.Where(current => current.WithdrawDateTime != null && current.WithdrawDateTime <= now)
			.Where(current => current.Wallet != null && current.Wallet.Token == walletToken)
			.Where(current => current.User != null && current.User.CellPhoneNumber == cellPhoneNumber)
			.Sum(current => current.Amount);

		// کل مبالغ قابل برداشت
		var withdrawBalance =
			allDeposite - allAntiDeposite;

		if (withdrawBalance < 0)
		{
			withdrawBalance = 0;
		}

		result.Data =
			withdrawBalance;

		return result;
	}
	#endregion /GetUserWithdrawBalance()
}
