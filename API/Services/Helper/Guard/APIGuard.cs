﻿using Stall.Guard.System;

namespace API.Services.Helper.Guard
{
    public class APIGuard : StdGuard
    {
        public GuardResult IsAdmissible(double argument)
        {
            if (argument < 0)
                return new GuardResult(Status.Failure, "Argument is negative.");

            return new GuardResult(Status.Success, string.Empty);
        }

        public GuardResult IsAdmissible(string parameter, double argument)
        {
            if (argument < 0)
            {
                var info = string.Format("{0} is negative.", parameter);
                return new GuardResult(Status.Failure, info);
            }
            return new GuardResult(Status.Success, string.Empty);
        }
    }
}