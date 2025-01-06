﻿using System.Globalization;

namespace Courses.Business.Abstract.Constants;
public static class DefaultUsers
{
    public partial class Admin
    {
        public const string Id = "019409bf-3ae7-7cdf-995b-db4620f2ff5f";
        public const string SecurityStamp = "019409c1-af2c-7e25-bc46-da6e10412d65";
        public const string ConcurrencyStamp = "019409C1-DB8B-7B6F-A8A1-8E35FB4D0748";
        public const string Email = "admin@courses.edu";
        public const string UserName = "admin";
        public const string Password = "P@ssword123";
        public const string Name = nameof(Admin);
    }
}
