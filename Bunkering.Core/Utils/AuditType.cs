﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Utils
{
    public enum AuditType
    {
        None = 0,
        Create = 1,
        Update = 2,
        Delete = 3
    }

    public enum AppStatus
    {
        Initiated,
        PaymentPending,
        PaymentCompleted,
        Processing,
        Rejected,
        Completed,
        PaymentRejected
    }

    public enum AppActions
    {
        Initiate,
        GenereatePayment,
        ConfirmPayment,
        UploadDocument,
        Submit,
        Resubmit,
        Approve,
        Reject,
        ScheduleInspection,
        ApproveInspection,
        AcceptInspection,
        FinalApproval,
        RejectInspection
    }
}
