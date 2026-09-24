// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using Bodoconsult.App.Helpers;
using Bodoconsult.Web.Mail.Helpers;

namespace BodoWebMailer.Business.Helpers;

public class PasswordHelper
{

    public static string Encrypt(string raw)
    {
        return PasswordHandler.Encrypt(raw);
    }

    public static string Decrypt(string crypted)
    {
        return PasswordHandler.Decrypt(crypted);
    }

}