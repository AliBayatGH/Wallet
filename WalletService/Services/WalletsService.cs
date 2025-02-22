﻿using System.Linq;

namespace Server.Services;

public static class WalletsService : object
{
	static WalletsService()
	{
	}

	#region CheckAndGetWalletByToken()
	public static Dtat.Result<Domain.Wallet>
		CheckAndGetWalletByToken(Data.DatabaseContext databaseContext, System.Guid token)
	{
		var result =
			new Dtat.Result<Domain.Wallet>();

		var wallet =
			databaseContext.Wallets
			.Where(current => current.Token == token)
			.FirstOrDefault();

		if (wallet == null)
		{
			var errorMessage = string.Format
				(format: Resources.Messages.Errors.ThereIsNotAnyItemWithThisToken,
				arg0: nameof(wallet));

			result.AddErrorMessages
				(message: errorMessage);

			return result;
		}

		if (wallet.IsActive == false)
		{
			var errorMessage = string.Format
				(format: Resources.Messages.Errors.TheItemIsNotActive,
				arg0: nameof(wallet));

			result.AddErrorMessages
				(message: errorMessage);

			return result;
		}

		result.Data = wallet;

		return result;
	}
	#endregion /CheckAndGetWalletByToken()
}
