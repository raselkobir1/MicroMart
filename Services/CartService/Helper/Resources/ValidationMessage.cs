namespace Inventory.API.Helper.Resources
{
    public static class ValidationMessage
    {
        public const string MenuId_DuplicateEntry = "Duplicate Menu Entry ({0})!";
        public const string Menu_InvalidIds = "Invalid Menu Ids ({0})";
        public const string Menu_IsNotALeaf = "'{0} ({1})' Is Not A Leaf Menu";
        public const string Role_InvalidId = "Invalid Role Id ({0})";
        public const string Role_InvalidIds = "Invalid Role Ids ({0})";
        public const string EmailAlreadyExists = "Email already exists.";
        public const string User_NoSelfStatusUpdate = "Cannot change self active status.";
        public const string User_NoSelfRoleUpdate = "Cannot change self role.";
        public const string User_NoSelfEmailUpdate = "Cannot change self email.";
        public const string Role_NameNotFound = "Role name not found!";
        public const string Service_InvalidId = "Invalid Service Id ({0})";
        public const string Menu_InvalidServiceId = "Menu ({0}({1})) Is Not Under Service ({2})";
        public const string Currency_InvalidId = "Invalid Currency Id ({0})";
        public const string SupplierType_InvalidId = "Invalid SupplierType Id ({0})";
        public const string Airline_InvalidId = "Invalid Airline Id ({0})";
        public const string File_SaveFailed = "Failed To Save File. Reason - {0}";
        public const string TopAirline_DuplicateEntry = "This top airline already exists, please try different.";
        public const string TopAirline_DuplicatePriorityEntry = "Duplicate Priority Entry ({0})!";
        public const string TopAirline_InvalidId = "Invalid TopAirline Id ({0})";
        public const string Menu_InvalidParentId = "Can not set {0}({1}) as parent menu because it's a leaf menu!";
        public const string Menu_UnableToDeleteMenuBecauseOfChild = "Can not delete a menu if it has child menu!";
        public const string Menu_UnableToSetLeafBecauseOfChild = "Can not set a menu as leaf if a menu has child menu!";
        public const string User_InvalidId = "Invalid user id ({0})";
        public const string Module_InvalidId = "Invalid Module id ({0})";
        public const string Company_InvalidId = "Invalid company id ({0})";
        public const string Organization_InvalidId = "Invalid organization id ({0})";
        public const string SupplierCode_Exits = "Supplier code [{0}] already exists, please try a different.";
        public const string Country_InvalidId = "Invalid Country Id ({0})";
        public const string Banner_DuplicateEntry = "Duplicate BannerId ({0})!";
        public const string Banner_DuplicatePriorityEntry = "Duplicate Priority Entry ({0})!";
        public const string Banner_InvalidId = "Invalid Banner Id ({0})";
        public const string PasswordNotMatch = "Password Change Failed: The old password you entered is incorrect. Please check your old password and try again.";
        public const string Supplier_InvalidId = "Invalid Supplier Id ({0})";
        public const string SuplierIdExist = "Resource already exists for suplierId ({0})";
        public const string PaymentMethod_InvalidId = "Invalid Payment Method Id ({0})";
        public const string PaymentMethod_DuplicateEntry = "Duplicate PaymentMethod Id({0})!";
        public const string PaymentMethod_DuplicatePriorityEntry = "Duplicate Priority Entry ({0})!";
        public const string Template_NotificationAreaAlreadyExist = "Notification Area Already Exist";
        public const string NotificationAreaIsInActive = "Notification Area Is InActive";
        public const string Country_IdExists = "Duplicate Country Id ({0})!";
        public const string SupplierNameExists = "Supplier name [{0}] already exists, please try a different.";
        public const string CountryAlreadyExists = "This country already exists, please try different.";
        public const string BookingIdAlreadyExist = "Booking Id Already Exist";
        public const string SupplierAlreadyExists = "Supplier already exists.";
        public const string DestinationAirportNotExists = "Airport Destination Not Exists";
        public const string OriginAirportNotExists = "Airport Origin Not Exists";
        public const string TripTypeNotExist = "Trip Type Not Exist";
        public const string BookingPassenger_InvalidIds = "Invalid BookingPassenger Ids ({0})";
        public const string InvalidExportType = "Chose a proper type(Excel or CSV) for export report.";
        public const string InvalidPaymentMethod = "Invalid payment method.";
        public const string BookingId_NotExists = "Booking Id Is Not Exist";
        public const string InvalidPaymentMethodCardInformation = "Invalid payment method card information";
        public const string HeroBannerExist = "Header and Sub text alrady exist!";
        public const string InvalidPaymentMethodBankInfo = "Invalid payment method bank information";
        public const string AddedDifferentServiceId = "Parent has already a differnt service Id";
        public const string RefundNegetiveAmount = "Totol Refund Amount Cannot Be Negetive";
        public const string ActionAlreadyExist = "Action Already Exist";
        public const string samePassword = "Your new password must be different from the current one.";
        public const string ResourcePathNotExist = "Resource path not exist";
    }
}
