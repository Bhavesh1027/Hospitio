export interface CustomClaimTypes {
    permission: string;
    scopes: string;
}

export enum AgeCategory {
    Adult = 1,
    Child = 2
}

export enum MessageSenderEnum {
    Hospitio = 1,
    Customer = 2,
    Guest = 3
}

export enum MsgReqTypeEnum {
    message = 1,
    request = 2,
    journeyMessage = 3,
    alertMessage = 4,
    digitalAssitant = 5,
    welcomeMessage = 6
}

export enum ChatUserTypeEnum {
    HospitioUser = 1,
    CustomerUser = 2,
    CustomerGuest = 3,
    AnonymousUser = 4,
    ChatWidgetUser = 5
}

export enum RequestTypeEnum {
    Common = 1,
    EnhanceYourStay = 2
}

export enum UserTypeEnum {
    Hospitio = 1,
    Customer = 2,
    Guest = 3,
    AnonymousUser = 4,
    ChatWidgetUser = 5
}

export enum Badge {
    New = 1,
    BestSeller = 2,
    Recommended = 3,
    Special = 4,
    Free = 5,
    NoBadge = 6
}

export enum ButtonType {
    Purchase = 1,
    CalltoAction = 2,
    Informative = 3
}

export enum ChargeType {
    PerUnit = 1,
    PerNight = 2,
    PerPerson = 3,
    PerPersonPerNight = 4
}

export enum GuestAppBuilderStatus {
    Completed = 1,
    InProcess = 2,
    NeedToDO = 3
}

export enum GuestRequestStatusEnum {
    Completed = 1,
    InProgress = 2
}

export enum GuestRequestTypeEnum {
    EnhanceYourStay = 1,
    Reception = 2,
    Housekeeping = 3,
    RoomService = 4,
    Concierge = 5
}

export enum GuestsStatusEnum {
    Expected = 1,
    InHouse = 2,
    CheckOut = 3
}

export enum JourneyStepTempleteTypeEnums {
    PostBooking = 1,
    OnlineCheckIn = 2,
    GuestPWA = 3,
    PreArrival = 4,
    InHouse = 5,
    PostArrival = 6
}

export enum MessageSourceEnum {
    WebChat = 1,
    WebChatRequest = 2,
    whatsapp = 3,
    sms = 4,
    viber_service = 5,
    messenger = 6
}

export enum PMS {
    None = 1,
    SiteMinder = 2,
    Avantio = 3,
    LODGIFY = 4,
    protel = 5,
    Loggia = 6
}

export enum TicketPriority {
    High = 1,
    Medium = 2,
    Low = 3
}

export enum TicketStatus {
    Pending = 1,
    Assigned = 2,
    Closed = 3
}

export enum UploadDocumentType {
    profile = 0,
    propertyservice = 1,
    yourstayitems = 2,
    articles = 3,
    documentattachment = 4,
    basic = 5,
    propertyinfogallary = 6,
    enhanceyourstay = 7,
    appBuilderImages = 8,
    guestProfile = 9,
    communicationattachment = 10,
    customerProfile = 11,
    customerGuestXMLSheet = 12,
    downloadMusementData = 13,
    downloadCustomerLeads = 14,
    downloadVonageRecordsReports = 15,
    downloadTaxiTransferData = 16
}

export enum UserLevel {
    Guest = 0,
    SuperAdmin = 1,
    CEO = 2,
    DeptManager = 3,
    GroupLeader = 4,
    Staff = 5
}

export interface AccessToken {
    jwt: string;
}

export interface BaseResponseOut {
    message: string;
    errors: string[];
}

export interface ChatHubConnectionToken {
    token: AccessToken;
    expires: DateTimeUtcUnixEpoch;
}

export interface CustomerUserRefreshToken {
    token: string;
    tokenId: number;
    expires: DateTimeUtcUnixEpoch;
}

export interface DateTimeUtcUnixEpoch {
    dateTimeUTC: string;
    unixEpoch: number;
}

export interface BaseSearchFilterOptions {
    pageNo: number;
    pageSize: number;
    sort: string[];
    searchParam: string;
    searchColumn: string;
    searchValue: string;
    sortColumn: string;
    sortOrder: string;
}

export interface SearchResultBase {
    currentPage: number;
    pageCount: number;
    pageSize: number;
    rowCount: number;
}

export interface RefreshToken {
    token: string;
    tokenId: number;
    expires: DateTimeUtcUnixEpoch;
}

export namespace Account.ChangeCustomerPassword {
export interface ChangeCustomerPasswordIn {
    oldPassword: string;
    newPassword: string;
}

}
export namespace Account.ChangeCustomerPassword {
export interface ChangeCustomerPasswordOut extends BaseResponseOut {

}

}
export namespace Account.ChangeUserPassword {
export interface ChangeUserPasswordIn {
    oldPassword: string;
    newPassword: string;
}

}
export namespace Account.ChangeUserPassword {
export interface ChangeUserPasswordOut extends BaseResponseOut {

}

}
export namespace Account.CustomerLogin {
export interface CustomerLoginIn {
    email: string;
    password: string;
}

}
export namespace Account.CustomerLogin {
export interface CustomerLoginOut extends BaseResponseOut {
    accessToken: AccessToken;
    refreshToken: CustomerUserRefreshToken;
    chatHubConnectionAccessToken: ChatHubConnectionToken;
    isMuted: boolean;
}

}
export namespace Account.CustomerRefreshToken {
export interface CustomerRefreshTokenIn {
    tokenValue: string;
    tokenId: number;
}

}
export namespace Account.CustomerRefreshToken {
export interface CustomerRefreshTokenOut extends BaseResponseOut {
    accessToken: AccessToken;
    refreshToken: RefreshToken;
}

}
export namespace Account.CustomerRevokeToken {
export interface CustomerRevokeTokenIn {
    tokenValue: string;
    tokenId: number;
}

}
export namespace Account.CustomerRevokeToken {
export interface CustomerRevokeTokenOut extends BaseResponseOut {

}

}
export namespace Account.EncryptPasswords {
export interface EncryptPasswordsIn {

}

}
export namespace Account.EncryptPasswords {
export interface EncryptPasswordsOut extends BaseResponseOut {

}

}
export namespace Account.Login {
export interface LoginIn {
    email: string;
    password: string;
}

}
export namespace Account.Login {
export interface LoginOut extends BaseResponseOut {
    userId: number;
    accessToken: AccessToken;
    refreshToken: RefreshToken;
    chatHubConnectionAccessToken: ChatHubConnectionToken;
}

}
export namespace Account.RefreshToken {
export interface RefreshTokenIn {
    tokenValue: string;
    tokenId: number;
}

}
export namespace Account.RefreshToken {
export interface RefreshTokenOut extends BaseResponseOut {
    accessToken: AccessToken;
    refreshToken: RefreshToken;
}

}
export namespace Account.ResetPassword {
export interface ResetPasswordIn {
    token: string;
    password: string;
}

}
export namespace Account.ResetPassword {
export interface ResetPasswordOut extends BaseResponseOut {
    id: number;
    accessToken: AccessToken | null;
    refreshToken: ResetPasswordRefreshTokenOut | null;
}

export interface ResetPasswordRefreshTokenOut {
    token: string;
    tokenId: number;
    expires: DateTimeUtcUnixEpoch;
}

export interface ResetPasswordTokenDetailOut {
    id: number;
    token: string;
    expiresUtc: string;
    createdByIp: string | null;
}

export interface ResetPasswordTokenOut {
    accessToken: AccessToken | null;
    chatHubConnectionAccessToken: ChatHubConnectionToken;
    refreshToken: ResetPasswordRefreshTokenOut | null;
}

}
export namespace Account.ResetPasswordConfirmation {
export interface ResetPasswordConfirmationIn {
    email: string;
    isUser: boolean;
}

}
export namespace Account.ResetPasswordConfirmation {
export interface ResetPasswordConfirmationOut extends BaseResponseOut {

}

}
export namespace Account.RevokeToken {
export interface RevokeTokenIn {
    tokenValue: string;
    tokenId: number;
}

}
export namespace Account.RevokeToken {
export interface RevokeTokenOut extends BaseResponseOut {

}

}
export namespace AdminCustomerAlerts.CreateAdminCustomerAlerts {
export interface CreateAdminCustomerAlertsIn {
    msg: string | null;
    msgWaitTimeInMinutes: number | null;
    isActive: boolean | null;
}

}
export namespace AdminCustomerAlerts.CreateAdminCustomerAlerts {
export interface CreateAdminCustomerAlertsOut extends BaseResponseOut {
    createdAdminCustomerAlertsOut: CreatedAdminCustomerAlertsOut;
}

export interface CreatedAdminCustomerAlertsOut {
    id: number;
    msg: string | null;
    msgWaitTimeInMinutes: number | null;
    isActive: boolean | null;
}

}
export namespace AdminCustomerAlerts.DeleteAdminCustomerAlerts {
export interface DeleteAdminCustomerAlertsIn {
    id: number;
}

}
export namespace AdminCustomerAlerts.DeleteAdminCustomerAlerts {
export interface DeleteAdminCustomerAlertsOut extends BaseResponseOut {
    deletedAdminCustomerAlertsOut: DeletedAdminCustomerAlertsOut;
}

export interface DeletedAdminCustomerAlertsOut {
    id: number;
}

}
export namespace AdminCustomerAlerts.GetAdminCustomerAlerts {
export interface GetAdminCustomerAlertsIn {

}

}
export namespace AdminCustomerAlerts.GetAdminCustomerAlerts {
export interface GetAdminCustomerAlertsOut extends BaseResponseOut {
    adminCustomerAlertsOut: AdminCustomerAlertsOut[];
}

export interface AdminCustomerAlertsOut {
    id: number;
    msg: string | null;
    msgWaitTimeInMinutes: number | null;
    isActive: boolean | null;
}

}
export namespace AdminCustomerAlerts.UpdateAdminCustomerAlerts {
export interface UpdateAdminCustomerAlertsIn {
    id: number;
    msg: string | null;
    msgWaitTimeInMinutes: number | null;
    isActive: boolean | null;
}

}
export namespace AdminCustomerAlerts.UpdateAdminCustomerAlerts {
export interface UpdateAdminCustomerAlertsOut extends BaseResponseOut {
    updatedAdminCustomerAlertsOut: UpdatedAdminCustomerAlertsOut;
}

export interface UpdatedAdminCustomerAlertsOut {
    id: number;
    msg: string | null;
    msgWaitTimeInMinutes: number | null;
    isActive: boolean | null;
}

}
export namespace AdminStaffAlerts.CreateAdminStaffAlerts {
export interface CreateAdminStaffAlertsIn {
    name: string;
    platfrom: string;
    phoneCountry: string;
    phoneNumber: string;
    waitTimeInMintes: number | null;
    isActive: boolean | null;
    msg: string | null;
    userId: number | null;
}

}
export namespace AdminStaffAlerts.CreateAdminStaffAlerts {
export interface CreateAdminStaffAlertsOut extends BaseResponseOut {
    createdAdminStaffAlertsOut: CreatedAdminStaffAlertsOut;
}

export interface CreatedAdminStaffAlertsOut {
    id: number;
    name: string;
    platfrom: string;
    phoneCountry: string;
    phoneNumber: string;
    waitTimeInMintes: number | null;
    isActive: boolean | null;
    msg: string | null;
    userId: number | null;
    vonageTemplateStatus: string | null;
    vonageTemplateId: string | null;
    whatsappTemplateName: string | null;
}

}
export namespace AdminStaffAlerts.DeleteAdminStaffAlerts {
export interface DeleteAdminStaffAlertsIn {
    id: number;
}

}
export namespace AdminStaffAlerts.DeleteAdminStaffAlerts {
export interface DeleteAdminStaffAlertsOut extends BaseResponseOut {
    deletedAdminStaffAlertsOut: DeletedAdminStaffAlertsOut;
}

export interface DeletedAdminStaffAlertsOut {
    id: number;
}

}
export namespace AdminStaffAlerts.GetAdminStaffAlerts {
export interface GetAdminStaffAlertsIn {

}

}
export namespace AdminStaffAlerts.GetAdminStaffAlerts {
export interface GetAdminStaffAlertsOut extends BaseResponseOut {
    adminStaffAlertsOut: AdminStaffAlertsOut[];
}

export interface AdminStaffAlertsOut {
    id: number;
    name: string;
    platfrom: string;
    phoneCountry: string;
    phoneNumber: string;
    waitTimeInMintes: number | null;
    isActive: boolean | null;
    msg: string | null;
    userId: number | null;
    whatsappTemplateName: string;
    vonageTemplateStatus: string;
    vonageTemplateId: string;
}

}
export namespace AdminStaffAlerts.UpdateAdminStaffAlerts {
export interface UpdateAdminStaffAlertsIn {
    id: number;
    name: string;
    platfrom: string;
    phoneCountry: string;
    phoneNumber: string;
    waitTimeInMintes: number | null;
    isActive: boolean | null;
    msg: string | null;
    userId: number | null;
}

}
export namespace AdminStaffAlerts.UpdateAdminStaffAlerts {
export interface UpdateAdminStaffAlertsOut extends BaseResponseOut {
    updatedAdminStaffAlertsOut: UpdatedAdminStaffAlertsOut;
}

export interface UpdatedAdminStaffAlertsOut {
    id: number;
    name: string;
    platfrom: string;
    phoneCountry: string;
    phoneNumber: string;
    waitTimeInMintes: number | null;
    isActive: boolean | null;
    msg: string | null;
    userId: number | null;
    vonageTemplateStatus: string | null;
    vonageTemplateId: string | null;
    whatsappTemplateName: string | null;
}

}
export namespace BusinessTypes.GetBusinessTypes {
export interface GetBusinessTypesIn {

}

}
export namespace BusinessTypes.GetBusinessTypes {
export interface GetBusinessTypesOut extends BaseResponseOut {
    businessTypesOut: BusinessTypesOut[];
}

export interface BusinessTypesOut {
    id: number;
    bizType: string;
    isActive: boolean;
}

}
export namespace ConnectionDefinations.SyncConnectionDefinations {
export interface SyncConnectionDefinationsIn {
    items: ConnectionDefinition[];
}

export interface ConnectionDefinition {
    id: string | null;
    type: string | null;
    count: number;
    icon_Url: string | null;
    name: string | null;
    group: string | null;
    category: string | null;
    provider: string | null;
}

}
export namespace ConnectionDefinations.SyncConnectionDefinations {
export interface SyncConnectionDefinationsOut extends BaseResponseOut {

}

}
export namespace CustGuestPortalCheckInFormBuilder.CreateCustomerGuestPortalCheckInFormBuilder {
export interface CreateCustomerGuestPortalCheckInFormBuilderIn {
    customerId: number;
    customerReservationId: number;
    firstname: string | null;
    lastname: string | null;
    email: string | null;
    picture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    country: string | null;
    language: string | null;
    idProof: string | null;
    idProofType: string | null;
    idProofNumber: string | null;
    blePinCode: string | null;
    pin: string | null;
    street: string | null;
    streetNumber: string | null;
    city: string | null;
    postalcode: string | null;
    arrivalFlightNumber: string | null;
    departureAirline: string | null;
    departureFlightNumber: string | null;
    signature: string | null;
    roomNumber: string | null;
    termsAccepted: boolean | null;
    firstJourneyStep: number | null;
    rating: number | null;
    dateOfBirth: string | null;
    vat: string | null;
    ageCategory: number | null;
}

}
export namespace CustGuestPortalCheckInFormBuilder.CreateCustomerGuestPortalCheckInFormBuilder {
export interface CreateCustomerGuestPortalCheckInFormBuilderOut extends BaseResponseOut {
    createdCustomerGuestsCheckInFormBuilderOut: CreatedCustomerGuestsCheckInFormBuilderOut;
}

export interface CreatedCustomerGuestsCheckInFormBuilderOut {
    guestId: number;
    link: string;
}

}
export namespace CustGuestPortalCheckInFormBuilder.EditCustomerGuestPortalCheckInGuest {
export interface EditCustomerGuestPortalCheckInGuestIn {
    id: number;
    ageCategory: number | null;
    firstname: string | null;
    lastname: string | null;
    email: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
}

}
export namespace CustGuestPortalCheckInFormBuilder.EditCustomerGuestPortalCheckInGuest {
export interface EditCustomerGuestPortalCheckInGuestOut extends BaseResponseOut {
    updatedCustomerGuestOut: UpdatedCustomerGuestOut;
}

export interface UpdatedCustomerGuestOut {
    id: number;
    customerReservationId: number | null;
    firstname: string | null;
    lastname: string | null;
    email: string | null;
    picture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    country: string | null;
    language: string | null;
    idProof: string | null;
    idProofType: string | null;
    idProofNumber: string | null;
    blePinCode: string | null;
    pin: string | null;
    street: string | null;
    streetNumber: string | null;
    city: string | null;
    postalcode: string | null;
    arrivalFlightNumber: string | null;
    departureAirline: string | null;
    departureFlightNumber: string | null;
    signature: string | null;
    roomNumber: string | null;
    termsAccepted: boolean | null;
    firstJourneyStep: number | null;
    rating: number | null;
    dateOfBirth: string | null;
    vat: string | null;
    isActive: boolean | null;
}

}
export namespace CustGuestPortalCheckInFormBuilder.EditCustomerGuestPortalCheckInReservation {
export interface EditCustomerGuestPortalCheckInReservationIn {
    id: number;
    noOfGuestAdults: number | null;
    noOfGuestChilderns: number | null;
}

}
export namespace CustGuestPortalCheckInFormBuilder.EditCustomerGuestPortalCheckInReservation {
export interface EditCustomerGuestPortalCheckInReservationOut extends BaseResponseOut {
    updatedCustomerReservationOut: UpdatedCustomerReservationOut;
}

export interface UpdatedCustomerReservationOut {
    id: number;
    customerId: number | null;
    uuid: string | null;
    reservationNumber: string | null;
    source: string | null;
    noOfGuestAdults: number | null;
    noOfGuestChilderns: number | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    isActive: boolean | null;
}

}
export namespace CustGuestPortalCheckInFormBuilder.GetCustomerGuestCheckInFormBuilderIcon {
export interface GetCustomerGuestCheckInFormBuilderIconIn {
    customerId: number | null;
}

}
export namespace CustGuestPortalCheckInFormBuilder.GetCustomerGuestCheckInFormBuilderIcon {
export interface GetCustomerGuestCheckInFormBuilderIconOut extends BaseResponseOut {
    customerGuestCheckInFormBuilderIconOut: CustomerGuestCheckInFormBuilderIconOut;
}

export interface CustomerGuestCheckInFormBuilderIconOut {
    jsonData: string | null;
}

}
export namespace CustGuestPortalCheckInFormBuilder.GetCustomerGuestPortalCheckInFormBuilder {
export interface GetCustomerGuestPortalCheckInFormBuilderIn {
    guestId: number;
    reservationId: number;
}

}
export namespace CustGuestPortalCheckInFormBuilder.GetCustomerGuestPortalCheckInFormBuilder {
export interface GetCustomerGuestPortalCheckInFormBuilderOut extends BaseResponseOut {
    getCustomerReservationResponseOut: GetCustomerGuestResponseOut;
}

export interface GetCustomerGuestResponseOut {
    isCoGuest: boolean | null;
    name: string | null;
    buidlerId: number | null;
    roomId: number | null;
    roomName: string | null;
    phoneNumber: string | null;
    currencyCode: string | null;
    taxiTransferCommission: number | null;
    getCustomerReservationResponseOut: GetCustomerReservationResponseOut;
}

export interface GetCustomerReservationResponseOut {
    id: number | null;
    customerId: number | null;
    uuid: string | null;
    reservationNumber: string | null;
    source: string | null;
    noOfGuestAdults: number | null;
    noOfGuestChildrens: number | null;
    checkinDate: string | null;
    checkoutDate: string;
    isCheckInCompleted: boolean | null;
    isSkipCheckIn: boolean | null;
    appAccessCode: string | null;
    phoneNumber: string | null;
    keyId: number | null;
    isActive: boolean | null;
    getCustomerGuestsCheckInFormBuilderResponseOut: GetCustomerGuestsCheckInFormBuilderResponseOut;
}

export interface GetCustomerGuestsCheckInFormBuilderResponseOut {
    id: number | null;
    customerId: number | null;
    color: string | null;
    name: string | null;
    stars: number | null;
    logo: string | null;
    appImage: string | null;
    splashScreen: string | null;
    isOnlineCheckInFormEnable: boolean | null;
    isRedirectToGuestAppEnable: boolean | null;
    submissionMail: string | null;
    termsLink: string | null;
    guestWelcomeMessage: string | null;
    isActive: boolean | null;
    businessType: string | null;
}

}
export namespace CustGuestPortalCheckInFormBuilder.GetCustomerGuestsByReservation {
export interface GetCustomerGuestsByReservationIn {
    reservationId: number;
}

}
export namespace CustGuestPortalCheckInFormBuilder.GetCustomerGuestsByReservation {
export interface GetCustomerGuestsByReservationOut extends BaseResponseOut {
    customerGuestsOut: CustomerGuestsByReservationOut[];
}

export interface CustomerGuestsByReservationOut {
    id: number;
    customerReservationId: number | null;
    firstname: string | null;
    lastname: string | null;
    email: string | null;
    picture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    ageCategory: number | null;
    isCoGuest: boolean | null;
    isCheckInCompleted: boolean | null;
}

}
export namespace CustGuestsCheckInFormBuilder.CreateCustomerGuestsCheckInFormBuilder {
export interface CreateCustomerGuestsCheckInFormBuilderIn {
    id: number;
    customerId: number;
    color: string | null;
    name: string | null;
    stars: number | null;
    logo: string | null;
    appImage: string | null;
    splashScreen: string | null;
    isOnlineCheckInFormEnable: boolean;
    isRedirectToGuestAppEnable: boolean;
    submissionMail: string | null;
    termsLink: string | null;
    isActive: boolean;
    guestWelcomeMessage: string | null;
    customerGuestsCheckInFormFieldIn: CustomerGuestsCheckInFormFieldIn[];
}

export interface CustomerGuestsCheckInFormFieldIn {
    name: string;
    fieldType: string;
    requiredFields: boolean;
    isActive: boolean;
    displayOrder: number | null;
}

}
export namespace CustGuestsCheckInFormBuilder.CreateCustomerGuestsCheckInFormBuilder {
export interface CreateCustomerGuestsCheckInFormBuilderOut extends BaseResponseOut {
    createdCustomerGuestsCheckInFormBuilderOut: CreatedCustomerGuestsCheckInFormBuilderOut;
}

export interface CreatedCustomerGuestsCheckInFormBuilderOut {
    id: number;
}

export interface GuestsCheckInFormBuilderJsonOut {
    short_name: string | null;
    name: string | null;
    scope: string | null;
    start_url: string | null;
    display: string | null;
    icons: dynamic[] | null;
    theme_color: string | null;
    background_color: string | null;
}

export interface GuestsCheckInFormBuilderJsonIconOut {
    src: string | null;
    type: string | null;
    sizes: string | null;
}

}
export namespace CustGuestsCheckInFormBuilder.EditCreateCustomerGuestsCheckInFormBuilder {
export interface EditCustomerGuestsCheckInFormBuilderIn {
    id: number;
    customerId: number;
    color: string | null;
    name: string | null;
    stars: number | null;
    logo: string | null;
    appImage: string | null;
    splashScreen: string | null;
    isOnlineCheckInFormEnable: boolean;
    isRedirectToGuestAppEnable: boolean;
    submissionMail: string | null;
    termsLink: string | null;
    isActive: boolean;
    guestWelcomeMessage: string | null;
    customerGuestsCheckInFormFieldIn: EditCustomerGuestsCheckInFormFieldIn[];
}

export interface EditCustomerGuestsCheckInFormFieldIn {
    name: string;
    fieldType: string;
    requiredFields: boolean;
    isActive: boolean;
    displayOrder: number | null;
}

}
export namespace CustGuestsCheckInFormBuilder.EditCreateCustomerGuestsCheckInFormBuilder {
export interface EditCustomerGuestsCheckInFormBuilderOut extends BaseResponseOut {
    editedCustomerGuestsCheckInFormBuilderOut: EditedCustomerGuestsCheckInFormBuilderOut;
}

export interface EditedCustomerGuestsCheckInFormBuilderOut {
    id: number;
}

}
export namespace CustGuestsCheckInFormBuilder.GetCustomerGuestsCheckInFormBuilder {
export interface GetCustomerGuestsCheckInFormBuilderIn {
    customerId: number;
}

}
export namespace CustGuestsCheckInFormBuilder.GetCustomerGuestsCheckInFormBuilder {
export interface GetCustomerGuestsCheckInFormBuilderOut extends BaseResponseOut {
    getCustomerGuestsCheckInFormBuilderResponseOut: GetCustomerGuestsCheckInFormBuilderResponseOut;
}

export interface GetCustomerGuestsCheckInFormBuilderResponseOut {
    id: number;
    customerId: number;
    color: string | null;
    name: string | null;
    stars: number | null;
    logo: string | null;
    appImage: string | null;
    splashScreen: string | null;
    isOnlineCheckInFormEnable: boolean;
    isRedirectToGuestAppEnable: boolean;
    submissionMail: string | null;
    termsLink: string | null;
    isActive: boolean;
    guestWelcomeMessage: string | null;
    getCustomerGuestsCheckInFormFieldsOut: GetCustomerGuestsCheckInFormFieldsOut[];
}

export interface GetCustomerGuestsCheckInFormFieldsOut {
    name: string;
    fieldType: string;
    requiredFields: boolean;
    isActive: boolean;
    displayOrder: number | null;
}

}
export namespace CustomerEnhanceYourStay.CreateCustomerEnhanceYourStay {
export interface CreateCustomerEnhanceYourStayIn {
    customerId: number;
    customerGuestAppBuilderId: number | null;
    categoryNames: CategoryName;
}

export interface CategoryName {
    categoryId: number;
    name: string | null;
    categoryDisplayOrder: number | null;
    isPublish: boolean | null;
    isDeleted: boolean | null;
    categoryItems: CategoryItem[];
}

export interface CategoryItem {
    categoryItemId: number;
    isActive: boolean | null;
    itemDisplayOrder: number | null;
    isPublish: boolean | null;
    isDeleted: boolean | null;
}

}
export namespace CustomerEnhanceYourStay.CreateCustomerEnhanceYourStay {
export interface CreateCustomerEnhanceYourStayOut extends BaseResponseOut {
    createdCustomerEnhanceYourStayCategoryOut: CreatedCustomerEnhanceYourStayOut;
}

export interface CreatedCustomerEnhanceYourStayOut {
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
}

}
export namespace CustomerEnhanceYourStay.DeleteCustomerEnhanceYourStay {
export interface DeleteCustomerEnhanceYourStayIn {
    categoryId: number;
}

}
export namespace CustomerEnhanceYourStay.DeleteCustomerEnhanceYourStay {
export interface DeleteCustomerEnhanceYourStayOut extends BaseResponseOut {
    deletedCustomerEnhanceYourStayCategoryOut: DeletedCustomerEnhanceYourStayOut;
}

export interface DeletedCustomerEnhanceYourStayOut {
    categoryId: number;
}

}
export namespace CustomerEnhanceYourStay.DisplayOrderCustomerEnhanceYourStay {
export interface DisplayOrderCustomerEnhanceYourStayIn {
    displayOrderCustomerEnhanceYourStay: DisplayOrderCustomerEnhanceYourStay[];
}

export interface DisplayOrderCustomerEnhanceYourStay {
    id: number | null;
    displayOrder: number | null;
}

}
export namespace CustomerEnhanceYourStay.DisplayOrderCustomerEnhanceYourStay {
export interface DisplayOrderCustomerEnhanceYourStayOut extends BaseResponseOut {

}

export interface CustomerEnhanceYourStayJsonOut {
    categoryId: number;
    name: string | null;
    categoryDisplayOrder: number | null;
    isPublish: boolean | null;
    displayOrder: number | null;
    categoryItems: CategoryItem[];
}

export interface CategoryItem {
    categoryItemId: number;
    isActive: boolean | null;
    itemDisplayOrder: number | null;
}

}
export namespace CustomerEnhanceYourStay.GetCustomerEnhanceYourStay {
export interface GetCustomerEnhanceYourStayIn {
    builderId: number | null;
}

}
export namespace CustomerEnhanceYourStay.GetCustomerEnhanceYourStay {
export interface GetCustomerEnhanceYourStayOut extends BaseResponseOut {
    customersEnhanceYourStayCategoriesOut: CustomerEnhanceYourStayOut[];
}

export interface CustomerEnhanceYourStayOut {
    id: number;
    customerGuestAppBuilderId: number | null;
    customerId: number | null;
    categoryName: string | null;
    isActive: boolean | null;
    displayOrder: number | null;
    isDeleted: boolean | null;
    customerGuestAppEnhanceYourStayItems: CustomerGuestAppEnhanceYourStayItems[];
}

export interface CustomerGuestAppEnhanceYourStayItems {
    id: number;
    customerGuestAppBuilderId: number | null;
    customerId: number | null;
    customerGuestAppBuilderCategoryId: number | null;
    badge: number | null;
    shortDescription: string | null;
    longDescription: string | null;
    buttonType: number | null;
    buttonText: string | null;
    chargeType: number | null;
    discount: number | null;
    price: number | null;
    currency: string | null;
    isActive: boolean | null;
    displayOrder: number | null;
    isDeleted: boolean | null;
    customerGuestAppEnhanceYourStayItemsImages: CustomerGuestAppEnhanceYourStayItemsImages[];
}

export interface CustomerGuestAppEnhanceYourStayItemsImages {
    id: number | null;
    customerGuestAppEnhanceYourStayItemId: number | null;
    itemsImages: string | null;
    disaplayOrder: number | null;
    isActive: boolean | null;
}

}
export namespace CustomerEnhanceYourStay.GetCustomerEnhanceYourStayByCategory {
export interface GetCustomerEnhanceYourStayByCategoryIn {
    categoryId: number | null;
}

}
export namespace CustomerEnhanceYourStay.GetCustomerEnhanceYourStayByCategory {
export interface GetCustomerEnhanceYourStayByCategoryOut extends BaseResponseOut {
    customersEnhanceYourStayCategoriesOut: CustomerEnhanceYourStayByCategoryOut[];
}

export interface CustomerEnhanceYourStayByCategoryOut {
    id: number;
    customerGuestAppBuilderId: number | null;
    customerId: number | null;
    categoryName: string | null;
    isActive: boolean | null;
    displayOrder: number | null;
    isDeleted: boolean | null;
    customerGuestAppEnhanceYourStayItems: CustomerGuestAppEnhanceYourStayItems[];
}

export interface CustomerGuestAppEnhanceYourStayItems {
    id: number;
    customerGuestAppBuilderId: number | null;
    customerId: number | null;
    customerGuestAppBuilderCategoryId: number | null;
    badge: number | null;
    shortDescription: string | null;
    longDescription: string | null;
    buttonType: number | null;
    buttonText: string | null;
    chargeType: number | null;
    discount: number | null;
    price: number | null;
    currency: string | null;
    isActive: boolean | null;
    displayOrder: number | null;
    isDeleted: boolean | null;
    customerGuestAppEnhanceYourStayItemsImages: CustomerGuestAppEnhanceYourStayItemsImages[];
}

export interface CustomerGuestAppEnhanceYourStayItemsImages {
    id: number | null;
    customerGuestAppEnhanceYourStayItemId: number | null;
    itemsImages: string | null;
    disaplayOrder: number | null;
    isActive: boolean | null;
}

}
export namespace CustomerEnhanceYourStayCategories.CreateCustomerEnhanceYourStayCategory {
export interface CreateCustomerEnhanceYourStayCategoryIn {
    customerId: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
}

}
export namespace CustomerEnhanceYourStayCategories.CreateCustomerEnhanceYourStayCategory {
export interface CreateCustomerEnhanceYourStayCategoryOut extends BaseResponseOut {
    createdCustomerEnhanceYourStayCategoryOut: CreatedCustomerEnhanceYourStayCategoryOut;
}

export interface CreatedCustomerEnhanceYourStayCategoryOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
}

}
export namespace CustomerEnhanceYourStayCategories.DeleteCustomerEnhanceYourStayCategory {
export interface DeleteCustomerEnhanceYourStayCategoryIn {
    id: number;
}

}
export namespace CustomerEnhanceYourStayCategories.DeleteCustomerEnhanceYourStayCategory {
export interface DeleteCustomerEnhanceYourStayCategoryOut extends BaseResponseOut {
    deletedCustomerEnhanceYourStayCategoryOut: DeletedCustomerEnhanceYourStayCategoryOut;
}

export interface DeletedCustomerEnhanceYourStayCategoryOut {
    id: number;
}

}
export namespace CustomerEnhanceYourStayCategories.GetCustomerEnhanceYourStayCategories {
export interface GetCustomerEnhanceYourStayCategoriesIn {
    searchValue: string | null;
    pageNo: number;
    pageSize: number;
    sortColumn: string | null;
    sortOrder: string | null;
    customerId: number;
}

}
export namespace CustomerEnhanceYourStayCategories.GetCustomerEnhanceYourStayCategories {
export interface GetCustomerEnhanceYourStayCategoriesOut extends BaseResponseOut {
    customersEnhanceYourStayCategoriesOut: CustomerEnhanceYourStayCategoriesOut[];
}

export interface CustomerEnhanceYourStayCategoriesOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    filteredCount: number | null;
}

}
export namespace CustomerEnhanceYourStayCategories.GetCustomerEnhanceYourStayCategoriesWithRelation {
export interface GetCustomerEnhanceYourStayCategoriesWithRelationIn {
    searchValue: string | null;
    pageNo: number;
    pageSize: number;
    sortColumn: string | null;
    sortOrder: string | null;
    customerId: number;
}

}
export namespace CustomerEnhanceYourStayCategories.GetCustomerEnhanceYourStayCategoriesWithRelation {
export interface GetCustomerEnhanceYourStayCategoriesWithRelationOut extends BaseResponseOut {
    customersEnhanceYourStayCategoriesWithRelationOut: CustomerEnhanceYourStayCategoriesWithRelationOut[];
}

export interface CustomerEnhanceYourStayCategoriesWithRelationOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    customerGuestAppEnhanceYourStayItems: CustomerGuestAppEnhanceYourStayItemsRelationOut[];
}

export interface CustomerGuestAppEnhanceYourStayItemsRelationOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    customerGuestAppBuilderCategoryId: number | null;
    badge: number | null;
    shortDescription: string | null;
    longDescription: string | null;
    buttonType: number | null;
    buttonText: string | null;
    chargeType: number | null;
    discount: number | null;
    price: number | null;
    currency: string | null;
}

}
export namespace CustomerEnhanceYourStayCategories.GetCustomerEnhanceYourStayCategoryById {
export interface GetCustomerEnhanceYourStayCategoryByIdIn {
    id: number;
}

}
export namespace CustomerEnhanceYourStayCategories.GetCustomerEnhanceYourStayCategoryById {
export interface GetCustomerEnhanceYourStayCategoryByIdOut extends BaseResponseOut {
    customerEnhanceYourStayCategoryByIdOut: CustomerEnhanceYourStayCategoryByIdOut;
}

export interface CustomerEnhanceYourStayCategoryByIdOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
}

}
export namespace CustomerEnhanceYourStayCategories.UpdateCustomerEnhanceYourStayCategory {
export interface UpdateCustomerEnhanceYourStayCategoryIn {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
}

}
export namespace CustomerEnhanceYourStayCategories.UpdateCustomerEnhanceYourStayCategory {
export interface UpdateCustomerEnhanceYourStayCategoryOut extends BaseResponseOut {
    updatedCustomerEnhanceYourStayCategoryOut: UpdatedCustomerEnhanceYourStayCategoryOut;
}

export interface UpdatedCustomerEnhanceYourStayCategoryOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
}

}
export namespace CustomerEnhanceYourStayItems.CreateCustomerEnhanceYourStayItem {
export interface CreateCustomerEnhanceYourStayItemIn {
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    customerGuestAppBuilderCategoryId: number | null;
    badge: number | null;
    shortDescription: string | null;
    longDescription: string | null;
    buttonType: number | null;
    buttonText: string | null;
    chargeType: number | null;
    discount: number | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
    itemsImages: CreateEnhanceYourStayItemAttachementIn[];
    customerEnhanceYourStayCategoryItemExtra: CreateEnhanceYourStayCategoryItemExtraIn[];
}

export interface CreateEnhanceYourStayItemAttachementIn {
    itemsImage: string | null;
    displayOrder: number | null;
    isDeleted: boolean | null;
}

export interface CreateEnhanceYourStayCategoryItemExtraIn {
    queType: number | null;
    questions: string | null;
    optionValues: string | null;
    isDeleted: boolean | null;
}

}
export namespace CustomerEnhanceYourStayItems.CreateCustomerEnhanceYourStayItem {
export interface CreateCustomerEnhanceYourStayItemOut extends BaseResponseOut {
    createdCustomerEnhanceYourStayItemOut: CreatedCustomerEnhanceYourStayItemOut;
}

export interface CreatedCustomerEnhanceYourStayItemOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    customerGuestAppBuilderCategoryId: number | null;
    badge: number | null;
    shortDescription: string | null;
    longDescription: string | null;
    buttonType: number | null;
    buttonText: string | null;
    chargeType: number | null;
    discount: number | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    customerEnhanceYourStayCategoryItemsExtra: CreatedEnhanceYourStayCategoryItemExtraOut[];
    itemsImages: CreatedEnhanceYourStayItemImageOut[];
}

export interface CreatedEnhanceYourStayItemImageOut {
    id: number;
    customerGuestAppEnhanceYourStayItemId: number | null;
    itemsImages: string | null;
    disaplayOrder: number | null;
}

export interface CreatedEnhanceYourStayCategoryItemExtraOut {
    id: number | null;
    queType: number | null;
    questions: string | null;
    optionValues: string | null;
}

}
export namespace CustomerEnhanceYourStayItems.DeleteCustomerEnhanceYourStayItem {
export interface DeleteCustomerEnhanceYourStayItemIn {
    id: number;
}

}
export namespace CustomerEnhanceYourStayItems.DeleteCustomerEnhanceYourStayItem {
export interface DeleteCustomerEnhanceYourStayItemOut extends BaseResponseOut {
    deletedCustomerEnhanceYourStayItemOut: DeletedCustomerEnhanceYourStayItemOut;
}

export interface DeletedCustomerEnhanceYourStayItemOut {
    id: number;
}

}
export namespace CustomerEnhanceYourStayItems.GetCustomerEnhanceYourStayItemById {
export interface GetCustomerEnhanceYourStayItemByIdIn {
    id: number;
}

}
export namespace CustomerEnhanceYourStayItems.GetCustomerEnhanceYourStayItemById {
export interface GetCustomerEnhanceYourStayItemByIdOut extends BaseResponseOut {
    customerEnhanceYourStayItemByIdOut: CustomerEnhanceYourStayItemByIdOut;
}

export interface CustomerEnhanceYourStayItemByIdOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    customerGuestAppBuilderCategoryId: number | null;
    badge: number | null;
    shortDescription: string | null;
    longDescription: string | null;
    buttonType: number | null;
    buttonText: string | null;
    chargeType: number | null;
    discount: number | null;
    price: number | null;
    currency: string | null;
    isDeleted: boolean | null;
    isActive: boolean | null;
    customerEnhanceYourStayCategoryItemsExtra: EnhanceYourStayCategoryItemExtraOut[];
    itemsImages: EnhanceYourStayItemImageOut[];
}

export interface EnhanceYourStayItemImageOut {
    id: number;
    customerGuestAppEnhanceYourStayItemId: number | null;
    itemsImages: string | null;
    disaplayOrder: number | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
}

export interface EnhanceYourStayCategoryItemExtraOut {
    id: number | null;
    queType: number | null;
    questions: string | null;
    optionValues: string | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
}

}
export namespace CustomerEnhanceYourStayItems.GetCustomerEnhanceYourStayItems {
export interface GetCustomerEnhanceYourStayItemsIn {
    searchValue: string | null;
    pageNo: number;
    pageSize: number;
    sortColumn: string | null;
    sortOrder: string | null;
    customerId: number;
}

}
export namespace CustomerEnhanceYourStayItems.GetCustomerEnhanceYourStayItems {
export interface GetCustomerEnhanceYourStayItemsOut extends BaseResponseOut {
    customersEnhanceYourStayItemsOut: CustomerEnhanceYourStayItemsOut[];
}

export interface CustomerEnhanceYourStayItemsOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    customerGuestAppBuilderCategoryId: number | null;
    badge: number | null;
    shortDescription: string | null;
    longDescription: string | null;
    buttonType: number | null;
    buttonText: string | null;
    chargeType: number | null;
    discount: number | null;
    price: number | null;
    currency: string | null;
    filteredCount: number | null;
    isDeleted: boolean | null;
}

}
export namespace CustomerEnhanceYourStayItems.UpdateCustomerEnhanceYourStayItem {
export interface UpdateCustomerEnhanceYourStayItemIn {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    customerGuestAppBuilderCategoryId: number | null;
    badge: number | null;
    shortDescription: string | null;
    longDescription: string | null;
    buttonType: number | null;
    buttonText: string | null;
    chargeType: number | null;
    discount: number | null;
    price: number | null;
    currency: string | null;
    isPublish: boolean | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
    displayOrder: number | null;
    itemsImages: UpdateEnhanceYourStayItemAttachementIn[];
    customerEnhanceYourStayCategoryItemExtra: UpdateEnhanceYourStayCategoryItemExtraIn[];
}

export interface UpdateEnhanceYourStayItemAttachementIn {
    id: number | null;
    itemsImage: string | null;
    displayOrder: number | null;
    isActive: boolean | null;
    isPublish: boolean | null;
    isDeleted: boolean | null;
}

export interface UpdateEnhanceYourStayCategoryItemExtraIn {
    id: number | null;
    queType: number | null;
    questions: string | null;
    optionValues: string | null;
    isPublish: boolean | null;
    isActive: boolean | null;
    displayOrder: number | null;
    isDeleted: boolean | null;
}

}
export namespace CustomerEnhanceYourStayItems.UpdateCustomerEnhanceYourStayItem {
export interface UpdateCustomerEnhanceYourStayItemOut extends BaseResponseOut {
    updatedCustomerEnhanceYourStayItemOut: UpdatedCustomerEnhanceYourStayItemOut;
}

export interface UpdatedCustomerEnhanceYourStayItemOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    customerGuestAppBuilderCategoryId: number | null;
    badge: number | null;
    shortDescription: string | null;
    longDescription: string | null;
    buttonType: number | null;
    buttonText: string | null;
    chargeType: number | null;
    discount: number | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    customerEnhanceYourStayCategoryItemsExtra: UpdatedEnhanceYourStayCategoryItemExtraOut[];
    itemsImages: UpdatedEnhanceYourStayItemImageOut[];
}

export interface UpdatedEnhanceYourStayItemImageOut {
    id: number;
    customerGuestAppEnhanceYourStayItemId: number | null;
    itemsImages: string | null;
    disaplayOrder: number | null;
}

export interface UpdatedEnhanceYourStayCategoryItemExtraOut {
    id: number | null;
    queType: number | null;
    questions: string | null;
    optionValues: string | null;
}

}
export namespace CustomerGuest.CreateCustomerChatWidgetUser {
export interface CreateCustomerChatWidgetUserIn {
    encryptedCustomerId: string;
}

}
export namespace CustomerGuest.CreateCustomerChatWidgetUser {
export interface CreateCustomerChatWidgetUserOut extends BaseResponseOut {
    createdCustomerGuestOut: CreatedCustomerChatWidgetUserOut;
}

export interface CreatedCustomerChatWidgetUserOut {
    link: string;
    customerUserId: number;
    widgetUserId: number;
    cname: string | null;
    businessName: string | null;
}

}
export namespace CustomerGuest.CreateCustomerGuest {
export interface CreateCustomerGuestIn {
    customerReservationId: number | null;
    firstname: string | null;
    lastname: string | null;
    email: string | null;
    picture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    country: string | null;
    language: string | null;
    idProof: string | null;
    idProofType: string | null;
    idProofNumber: string | null;
    blePinCode: string | null;
    pin: string | null;
    street: string | null;
    streetNumber: string | null;
    city: string | null;
    postalcode: string | null;
    arrivalFlightNumber: string | null;
    departureAirline: string | null;
    departureFlightNumber: string | null;
    signature: string | null;
    roomNumber: string | null;
    termsAccepted: boolean | null;
    firstJourneyStep: number | null;
    rating: number | null;
    dateOfBirth: string | null;
    vat: string | null;
    isActive: boolean | null;
    bookingChannel: string | null;
    departingFlightDate: string | null;
}

}
export namespace CustomerGuest.CreateCustomerGuest {
export interface CreateCustomerGuestOut extends BaseResponseOut {
    createdCustomerGuestOut: CreatedCustomerGuestOut;
}

export interface CreatedCustomerGuestOut {
    id: number;
    customerReservationId: number | null;
    firstname: string | null;
    lastname: string | null;
    email: string | null;
    picture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    country: string | null;
    language: string | null;
    idProof: string | null;
    idProofType: string | null;
    idProofNumber: string | null;
    blePinCode: string | null;
    pin: string | null;
    street: string | null;
    streetNumber: string | null;
    city: string | null;
    postalcode: string | null;
    arrivalFlightNumber: string | null;
    departureAirline: string | null;
    departureFlightNumber: string | null;
    signature: string | null;
    roomNumber: string | null;
    termsAccepted: boolean | null;
    firstJourneyStep: number | null;
    rating: number | null;
    dateOfBirth: string | null;
    vat: string | null;
    isActive: boolean | null;
    link: string;
}

export interface GuestTokenOut {
    reservationId: number;
    customerId: string;
    guestId: number;
}

}
export namespace CustomerGuest.DeleteCustomerGuest {
export interface DeleteCustomerGuestIn {
    id: number;
}

}
export namespace CustomerGuest.DeleteCustomerGuest {
export interface DeleteCustomerGuestOut extends BaseResponseOut {
    deletedCustomerGuestOut: DeletedCustomerGuestOut;
}

export interface DeletedCustomerGuestOut {
    id: number;
}

}
export namespace CustomerGuest.DownloadCustomerGuest {
export interface DownloadCustomerGuestIn {

}

}
export namespace CustomerGuest.DownloadCustomerGuest {
export interface DownloadCustomerGuestOut extends BaseResponseOut {
    customerGuestsOut: string;
}

export interface DownloadCustomerGuestsOut {
    excelData: string | null;
}

export interface CustomerGuestDownloadOut {
    firstname: string | null;
    lastname: string | null;
    roomNumber: string | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    guestToken: string | null;
}

}
export namespace CustomerGuest.EditCustomerGuest {
export interface EditCustomerGuestIn {
    id: number;
    customerReservationId: number | null;
    firstname: string | null;
    lastname: string | null;
    email: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    country: string | null;
    pin: string | null;
    street: string | null;
    streetNumber: string | null;
    city: string | null;
    postalcode: string | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    blePinCode: string | null;
}

}
export namespace CustomerGuest.EditCustomerGuest {
export interface EditCustomerGuestOut extends BaseResponseOut {
    editedCustomerGuestOut: EditedCustomerGuestOut;
}

export interface EditedCustomerGuestOut {
    id: number;
    customerReservationId: number | null;
    firstName: string;
    lastName: string;
    email: string;
    phoneCountry: string;
    phoneNumber: string;
    country: string | null;
    pin: string | null;
    street: string | null;
    streetNumber: string | null;
    city: string | null;
    postalcode: string | null;
    blePinCode: string | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    createdAt: string | null;
    userType: string;
    userId: number;
    isActive: boolean | null;
}

}
export namespace CustomerGuest.GetCustomerGuestById {
export interface GetCustomerGuestByIdIn {
    id: number;
}

}
export namespace CustomerGuest.GetCustomerGuestById {
export interface GetCustomerGuestByIdOut extends BaseResponseOut {
    customerGuestByIdOut: CustomerGuestByIdOut;
}

export interface CustomerGuestByIdOut {
    id: number;
    customerReservationId: number | null;
    firstname: string | null;
    lastname: string | null;
    email: string | null;
    picture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    country: string | null;
    language: string | null;
    idProof: string | null;
    idProofType: string | null;
    idProofNumber: string | null;
    blePinCode: string | null;
    pin: string | null;
    street: string | null;
    streetNumber: string | null;
    city: string | null;
    postalcode: string | null;
    arrivalFlightNumber: string | null;
    departureAirline: string | null;
    departureFlightNumber: string | null;
    signature: string | null;
    roomNumber: string | null;
    termsAccepted: boolean | null;
    firstJourneyStep: number | null;
    rating: number | null;
    dateOfBirth: string | null;
    vat: string | null;
    isActive: boolean | null;
    bookingChannel: string | null;
    departingFlightDate: string | null;
    ageCategory: number | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    reservationNumber: string | null;
    isCheckInCompleted: boolean | null;
    isSkipCheckIn: boolean | null;
}

}
export namespace CustomerGuest.GetCustomerGuests {
export interface GetCustomerGuestsIn {
    searchValue: string;
    pageNo: number | null;
    pageSize: number | null;
    sortColumn: string | null;
    sortOrder: string | null;
}

}
export namespace CustomerGuest.GetCustomerGuests {
export interface GetCustomerGuestsOut extends BaseResponseOut {
    customerGuestsOut: CustomerGuestsOut[];
}

export interface CustomerGuestsOut {
    id: number;
    customerReservationId: number | null;
    firstname: string | null;
    lastname: string | null;
    roomNumber: string | null;
    guestStatus: number;
    checkinDate: string | null;
    checkoutDate: string | null;
    filteredCount: number;
    guestToken: string | null;
}

}
export namespace CustomerGuest.GetMainGuestByReservationId {
export interface GetMainGuestByReservationIdIn {
    reservationId: number;
}

}
export namespace CustomerGuest.GetMainGuestByReservationId {
export interface GetMainGuestByReservationIdOut extends BaseResponseOut {
    customerGuestByReservationId: CustomerGuestByReservationIdOut;
}

export interface CustomerGuestByReservationIdOut {
    id: number;
    customerReservationId: number | null;
    firstname: string | null;
    lastname: string | null;
    email: string | null;
    picture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    country: string | null;
    language: string | null;
    idProof: string | null;
    idProofType: string | null;
    idProofNumber: string | null;
    blePinCode: string | null;
    pin: string | null;
    street: string | null;
    streetNumber: string | null;
    city: string | null;
    postalcode: string | null;
    arrivalFlightNumber: string | null;
    departureAirline: string | null;
    departureFlightNumber: string | null;
    signature: string | null;
    roomNumber: string | null;
    termsAccepted: boolean | null;
    firstJourneyStep: number | null;
    rating: number | null;
    dateOfBirth: string | null;
    vat: string | null;
    isActive: boolean | null;
    bookingChannel: string | null;
    departingFlightDate: string | null;
}

}
export namespace CustomerGuest.SendGuestPdfMail {
export interface SendGuestPdfMailIn {
    guestId: string | null;
    customerId: string | null;
    htmlContent: string | null;
}

}
export namespace CustomerGuest.SendGuestPdfMail {
export interface SendGuestPdfMailOut extends BaseResponseOut {

}

}
export namespace CustomerGuest.SendWelcomeMessage {
export interface SendWelcomeMessageIn {
    guestId: number | null;
    guestPortal: string | null;
}

}
export namespace CustomerGuest.SendWelcomeMessage {
export interface SendWelcomeMessageOut extends BaseResponseOut {
    createdCustomerGuestOut: SendWelcomeMessageGuestOut;
}

export interface SendWelcomeMessageGuestOut {
    guestId: number | null;
    guestPortal: string | null;
}

}
export namespace CustomerGuest.UpdateCustomerGuest {
export interface UpdateCustomerGuestIn {
    id: number;
    customerReservationId: number | null;
    firstname: string | null;
    lastname: string | null;
    email: string | null;
    picture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    country: string | null;
    language: string | null;
    idProof: string | null;
    idProofType: string | null;
    idProofNumber: string | null;
    blePinCode: string | null;
    pin: string | null;
    street: string | null;
    streetNumber: string | null;
    city: string | null;
    postalcode: string | null;
    arrivalFlightNumber: string | null;
    departureAirline: string | null;
    departureFlightNumber: string | null;
    signature: string | null;
    roomNumber: string | null;
    termsAccepted: boolean | null;
    firstJourneyStep: number | null;
    rating: number | null;
    dateOfBirth: string | null;
    vat: string | null;
    isActive: boolean | null;
    bookingChannel: string | null;
    departingFlightDate: string | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    isCheckInCompleted: boolean | null;
    isSkipCheckIn: boolean | null;
}

}
export namespace CustomerGuest.UpdateCustomerGuest {
export interface UpdateCustomerGuestOut extends BaseResponseOut {
    updatedCustomerGuestOut: UpdatedCustomerGuestOut;
}

export interface UpdatedCustomerGuestOut {
    id: number;
    customerReservationId: number | null;
    firstname: string | null;
    lastname: string | null;
    email: string | null;
    picture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    country: string | null;
    language: string | null;
    idProof: string | null;
    idProofType: string | null;
    idProofNumber: string | null;
    blePinCode: string | null;
    pin: string | null;
    street: string | null;
    streetNumber: string | null;
    city: string | null;
    postalcode: string | null;
    arrivalFlightNumber: string | null;
    departureAirline: string | null;
    departureFlightNumber: string | null;
    signature: string | null;
    roomNumber: string | null;
    termsAccepted: boolean | null;
    firstJourneyStep: number | null;
    rating: number | null;
    dateOfBirth: string | null;
    vat: string | null;
    isActive: boolean | null;
}

}
export namespace CustomerGuestAlerts.CreateCustomerGuestAlerts {
export interface CreateCustomerGuestAlertsIn {
    officeHoursMsg: string | null;
    officeHoursMsgWaitTimeInMinutes: number | null;
    offlineHourMsg: string | null;
    offlineHoursMsgWaitTimeInMinutes: number | null;
    replyAtDiffPeriod: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerGuestAlerts.CreateCustomerGuestAlerts {
export interface CreateCustomerGuestAlertsOut extends BaseResponseOut {
    createdCustomerGuestAlertsOut: CreatedCustomerGuestAlertsOut;
}

export interface CreatedCustomerGuestAlertsOut {
    id: number;
    officeHoursMsg: string | null;
    officeHoursMsgWaitTimeInMinutes: number | null;
    offlineHourMsg: string | null;
    offlineHoursMsgWaitTimeInMinutes: number | null;
    replyAtDiffPeriod: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerGuestAlerts.DeleteCustomerGuestAlerts {
export interface DeleteCustomerGuestAlertsIn {
    id: number;
}

}
export namespace CustomerGuestAlerts.DeleteCustomerGuestAlerts {
export interface DeleteCustomerGuestAlertsOut extends BaseResponseOut {
    deletedCustomerGuestAlertsOut: DeletedCustomerGuestAlertsOut;
}

export interface DeletedCustomerGuestAlertsOut {
    id: number;
}

}
export namespace CustomerGuestAlerts.GetCustomerGuestAlertById {
export interface GetCustomerGuestAlertByIdIn {
    id: number;
}

}
export namespace CustomerGuestAlerts.GetCustomerGuestAlertById {
export interface GetCustomerGuestAlertByIdOut extends BaseResponseOut {
    customerGuestAlertByIdOut: CustomerGuestAlertByIdOut;
}

export interface CustomerGuestAlertByIdOut {
    id: number;
    customerId: number | null;
    officeHoursMsg: string | null;
    officeHoursMsgWaitTimeInMinutes: number | null;
    offlineHourMsg: string | null;
    offlineHoursMsgWaitTimeInMinutes: number | null;
    replyAtDiffPeriod: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerGuestAlerts.GetCustomerGuestAlerts {
export interface GetCustomerGuestAlertsIn {
    customerId: number;
}

}
export namespace CustomerGuestAlerts.GetCustomerGuestAlerts {
export interface GetCustomerGuestAlertsOut extends BaseResponseOut {
    customerGuestAlertsOut: CustomerGuestAlertsOut[];
}

export interface CustomerGuestAlertsOut {
    id: number;
    officeHoursMsg: string | null;
    officeHoursMsgWaitTimeInMinutes: number | null;
    offlineHourMsg: string | null;
    offlineHoursMsgWaitTimeInMinutes: number | null;
    replyAtDiffPeriod: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerGuestAlerts.UpdateCustomerGuestAlerts {
export interface UpdateCustomerGuestAlertsIn {
    id: number;
    officeHoursMsg: string | null;
    officeHoursMsgWaitTimeInMinutes: number | null;
    offlineHourMsg: string | null;
    offlineHoursMsgWaitTimeInMinutes: number | null;
    replyAtDiffPeriod: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerGuestAlerts.UpdateCustomerGuestAlerts {
export interface UpdateCustomerGuestAlertsOut extends BaseResponseOut {
    updatedCustomerGuestAlertsOut: UpdatedCustomerGuestAlertsOut;
}

export interface UpdatedCustomerGuestAlertsOut {
    id: number;
    customerId: number | null;
    officeHoursMsg: string | null;
    officeHoursMsgWaitTimeInMinutes: number | null;
    offlineHourMsg: string | null;
    offlineHoursMsgWaitTimeInMinutes: number | null;
    replyAtDiffPeriod: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerGuestAppBuilder.CreateCustomerGuestAppBuilder {
export interface CreateCustomerGuestAppBuilderIn {
    customerId: number | null;
    customerRoomNameId: number | null;
    message: string | null;
    secondaryMessage: string | null;
    localExperience: boolean | null;
    ekey: boolean | null;
    propertyInfo: boolean | null;
    enhanceYourStay: boolean | null;
    reception: boolean | null;
    housekeeping: boolean | null;
    roomService: boolean | null;
    concierge: boolean | null;
    transferServices: boolean | null;
    onlineCheckIn: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerGuestAppBuilder.CreateCustomerGuestAppBuilder {
export interface CreateCustomerGuestAppBuilderOut extends BaseResponseOut {
    createdCustomerGuestAppBuilderOut: CreatedCustomerGuestAppBuilderOut;
}

export interface CreatedCustomerGuestAppBuilderOut {
    id: number;
    customerId: number | null;
    customerRoomNameId: number | null;
    message: string | null;
    secondaryMessage: string | null;
    localExperience: boolean | null;
    ekey: boolean | null;
    propertyInfo: boolean | null;
    enhanceYourStay: boolean | null;
    reception: boolean | null;
    housekeeping: boolean | null;
    roomService: boolean | null;
    concierge: boolean | null;
    transferServices: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerGuestAppBuilder.DeleteCustomerGuestAppBuilder {
export interface DeleteCustomerGuestAppBuilderIn {
    id: number;
}

}
export namespace CustomerGuestAppBuilder.DeleteCustomerGuestAppBuilder {
export interface DeleteCustomerGuestAppBuilderOut extends BaseResponseOut {
    deletedCustomerGuestAppBuilderOut: DeletedCustomerGuestAppBuilderOut;
}

export interface DeletedCustomerGuestAppBuilderOut {
    id: number;
}

}
export namespace CustomerGuestAppBuilder.GetCustomerGuestAppBuilderByCustomerRoomId {
export interface GetCustomerGuestAppBuilderByCustomerRoomIdIn {
    roomId: number;
    customerId: number | null;
    userType: string | null;
}

}
export namespace CustomerGuestAppBuilder.GetCustomerGuestAppBuilderByCustomerRoomId {
export interface GetCustomerGuestAppBuilderByCustomerRoomIdOut extends BaseResponseOut {
    customerGuestAppBuilderByCustomerRoomIdOut: CustomerGuestAppBuilderByCustomerRoomIdOut;
}

export interface CustomerGuestAppBuilderByCustomerRoomIdOut {
    id: number;
    message: string | null;
    secondaryMessage: string | null;
    localExperience: boolean | null;
    ekey: boolean | null;
    propertyInfo: boolean | null;
    enhanceYourStay: boolean | null;
    reception: boolean | null;
    housekeeping: boolean | null;
    roomService: boolean | null;
    concierge: boolean | null;
    transferServices: boolean | null;
    onlineCheckIn: boolean | null;
    isActive: boolean | null;
    moduleServiceOut: ModuleServiceOut[];
}

export interface ModuleServiceOut {
    name: string | null;
    displayOrder: number | null;
    isDisable: boolean | null;
    image: string | null;
    items: number | null;
    categories: number | null;
    customerAppBuliderId: number | null;
}

}
export namespace CustomerGuestAppBuilder.GetCustomerGuestAppBuilderById {
export interface GetCustomerGuestAppBuilderByIdIn {
    id: number;
}

}
export namespace CustomerGuestAppBuilder.GetCustomerGuestAppBuilderById {
export interface GetCustomerGuestAppBuilderByIdOut extends BaseResponseOut {
    customerGuestAppBuilderByIdOut: CustomerGuestAppBuilderByIdOut;
}

export interface CustomerGuestAppBuilderByIdOut {
    id: number;
    customerId: number | null;
    customerRoomNameId: number | null;
    message: string | null;
    secondaryMessage: string | null;
    localExperience: boolean | null;
    ekey: boolean | null;
    propertyInfo: boolean | null;
    enhanceYourStay: boolean | null;
    reception: boolean | null;
    housekeeping: boolean | null;
    roomService: boolean | null;
    concierge: boolean | null;
    transferServices: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerGuestAppBuilder.GetCustomerGuestAppBuilders {
export interface GetCustomerGuestAppBuildersIn {
    customerId: number;
    searchColumn: string;
    searchValue: string;
    pageNo: number | null;
    pageSize: number | null;
    sortColumn: string | null;
    sortOrder: string | null;
}

}
export namespace CustomerGuestAppBuilder.GetCustomerGuestAppBuilders {
export interface GetCustomerGuestAppBuildersOut extends BaseResponseOut {
    customerGuestAppBuildersOut: CustomerGuestAppBuildersOut[];
}

export interface CustomerGuestAppBuildersOut {
    id: number;
    customerId: number | null;
    customerRoomNameId: number | null;
    message: string | null;
    secondaryMessage: string | null;
    localExperience: boolean | null;
    ekey: boolean | null;
    propertyInfo: boolean | null;
    enhanceYourStay: boolean | null;
    reception: boolean | null;
    housekeeping: boolean | null;
    roomService: boolean | null;
    concierge: boolean | null;
    transferServices: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerGuestAppBuilder.GetCustomerGuestToken {
export interface GetCustomerGuestTokenIn {
    id: string;
}

}
export namespace CustomerGuestAppBuilder.GetCustomerGuestToken {
export interface GetCustomerGuestTokenOut extends BaseResponseOut {
    getCustomerGuestTokenClass: GetCustomerGuestTokenClass;
}

export interface GetCustomerGuestTokenClass {
    customerGuestToken: string | null;
}

}
export namespace CustomerGuestAppBuilder.UpdateCustomerGuestAppBuilder {
export interface UpdateCustomerGuestAppBuilderIn {
    id: number;
    customerRoomNameId: number | null;
    message: string | null;
    secondaryMessage: string | null;
    localExperience: boolean | null;
    ekey: boolean | null;
    propertyInfo: boolean | null;
    enhanceYourStay: boolean | null;
    reception: boolean | null;
    housekeeping: boolean | null;
    roomService: boolean | null;
    concierge: boolean | null;
    transferServices: boolean | null;
    onlineCheckIn: boolean | null;
    isActive: boolean | null;
    isWork: number | null;
    jsonData: string | null;
    activeKey: boolean | null;
}

}
export namespace CustomerGuestAppBuilder.UpdateCustomerGuestAppBuilder {
export interface UpdateCustomerGuestAppBuilderOut extends BaseResponseOut {
    updatedCustomerGuestAppBuilderOut: UpdatedCustomerGuestAppBuilderOut;
}

export interface UpdatedCustomerGuestAppBuilderOut {
    id: number;
    customerRoomNameId: number | null;
    message: string | null;
    secondaryMessage: string | null;
    localExperience: boolean | null;
    ekey: boolean | null;
    propertyInfo: boolean | null;
    enhanceYourStay: boolean | null;
    reception: boolean | null;
    housekeeping: boolean | null;
    roomService: boolean | null;
    concierge: boolean | null;
    transferServices: boolean | null;
    onlineCheckIn: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerGuestAppEnhanceYourStayCategoryItemsExtra.CreateCustomerGuestAppEnhanceYourStayCategoryItemExtra {
export interface CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraIn {
    customerGuestAppEnhanceYourStayItemId: number;
    createCustomerGuestAppEnhanceYourStayCategoryItems: CreateCustomerGuestAppEnhanceYourStayCategoryItem[];
}

export interface CreateCustomerGuestAppEnhanceYourStayCategoryItem {
    id: number | null;
    queType: number | null;
    questions: string | null;
    optionValues: string | null;
}

}
export namespace CustomerGuestAppEnhanceYourStayCategoryItemsExtra.CreateCustomerGuestAppEnhanceYourStayCategoryItemExtra {
export interface CustomerGuestAppEnhanceYourStayCategoryItemExtraOut extends BaseResponseOut {
    createdCustomers: CreatedCustomersGuestAppEnhanceYourStayCategoryItemExtraOut;
}

export interface CreatedCustomersGuestAppEnhanceYourStayCategoryItemExtraOut {
    customerGuestAppEnhanceYourStayItemId: number | null;
}

}
export namespace CustomerGuestAppEnhanceYourStayCategoryItemsExtra.DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtra {
export interface DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraIn {
    customerGuestAppEnhanceYourStayItemId: number;
}

}
export namespace CustomerGuestAppEnhanceYourStayCategoryItemsExtra.DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtra {
export interface DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraOut extends BaseResponseOut {
    remove: RemoveEnhanceYourStayCategoryItemExtraOut;
}

export interface RemoveEnhanceYourStayCategoryItemExtraOut {
    customerGuestAppEnhanceYourStayItemId: number;
}

}
export namespace CustomerGuestAppEnhanceYourStayCategoryItemsExtra.GetCustomerGuestAppEnhanceYourStayItemsExtraByStayId {
export interface GetCustomerGuestAppEnhannceYourStayItemExtraIn {
    customerGuestAppEnhanceYourStayItemId: number;
}

}
export namespace CustomerGuestAppEnhanceYourStayCategoryItemsExtra.GetCustomerGuestAppEnhanceYourStayItemsExtraByStayId {
export interface GetCustomerGuestAppEnhannceYourStayItemExtraOut extends BaseResponseOut {
    customers: CustomersGuestAppEnhanceYourStayCategoryItemsExtraOut[];
}

export interface CustomersGuestAppEnhanceYourStayCategoryItemsExtraOut {
    id: number | null;
    customerGuestAppEnhanceYourStayItemId: number | null;
    queType: number | null;
    questions: string | null;
    optionValues: string | null;
    isActive: boolean | null;
}

}
export namespace CustomerGuestPMS.CreateCustomerGuestPMS {
export interface CreateCustomerGuestPMSIn {
    documentAttachment: any;
    documentType: string | null;
    containerName: string | null;
    title: string | null;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    mobileNumber: string | null;
    street: string | null;
    postalCode: string | null;
    city: string | null;
    country: string | null;
    reservationNumber: string | null;
    arrivalDate: string | null;
    departureDate: string | null;
    vATNumber: string | null;
    documentName: string | null;
}

}
export namespace CustomerGuestPMS.CreateCustomerGuestPMS {
export interface CreateCustomerGuestPMSOut extends BaseResponseOut {
    createdCustomerGuestPMSOut: CreatedCustomerGuestPMSOut;
}

export interface CreatedCustomerGuestPMSOut {
    firstName: string | null;
    lastName: string | null;
    documentName: string | null;
    email: string | null;
    fileName: string;
}

}
export namespace CustomerHouseKeeping.CreateCustomerHouseKeeping {
export interface CreateCustomerHouseKeepingIn {
    customerHouseKeepingCategories: CreateCustomerHouseKeepingCategoryIn[];
}

export interface CreateCustomerHouseKeepingCategoryIn {
    customerId: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    customerHouseKeepingItems: CreateCustomerHouseKeepingItemIn[];
}

export interface CreateCustomerHouseKeepingItemIn {
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
}

}
export namespace CustomerHouseKeeping.CreateCustomerHouseKeeping {
export interface CreateCustomerHouseKeepingOut extends BaseResponseOut {
    createdCustomerHouseKeepingOut: CreatedCustomerHouseKeepingOut[];
}

export interface CreatedCustomerHouseKeepingOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
}

}
export namespace CustomerHouseKeeping.DeleteCustomerHouseKeeping {
export interface DeleteCustomerHouseKeepingIn {
    id: number;
}

}
export namespace CustomerHouseKeeping.DeleteCustomerHouseKeeping {
export interface DeleteCustomerHouseKeepingOut extends BaseResponseOut {
    deletedCustomerHouseKeepingOut: DeletedCustomerHouseKeepingOut;
}

export interface DeletedCustomerHouseKeepingOut {
    id: number;
}

}
export namespace CustomerHouseKeeping.DisplayOrderCustomerHouseKeeping {
export interface DisplayOrderCustomerHouseKeepingIn {
    displayOrderCustomerHouseKeepings: DisplayOrderCustomerHouseKeeping[];
}

export interface DisplayOrderCustomerHouseKeeping {
    id: number | null;
    displayOrder: number | null;
}

}
export namespace CustomerHouseKeeping.DisplayOrderCustomerHouseKeeping {
export interface DisplayOrderCustomerHouseKeepingOut extends BaseResponseOut {

}

export interface DisplayOrderCustomerHouseKeepingJsonOut {
    id: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
    customerHouseKeepingItems: CustomerHouseKeepingItem[];
}

export interface CustomerHouseKeepingItem {
    id: number;
    customerGuestAppBuilderId: number | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    isDeleted: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerHouseKeeping.GetCustomerHouseKeepingWithRelation {
export interface GetCustomerHouseKeepingWithRelationIn {
    appBuilderId: number;
}

}
export namespace CustomerHouseKeeping.GetCustomerHouseKeepingWithRelation {
export interface GetCustomerHouseKeepingWithRelationOut extends BaseResponseOut {
    customerHouseKeepingWithRelationOut: CustomerHouseKeepingWithRelationOut[];
}

export interface CustomerHouseKeepingWithRelationOut {
    id: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
    customerHouseKeepingItems: CustomerHouseKeepingItemsRelationOut[];
}

export interface CustomerHouseKeepingItemsRelationOut {
    id: number;
    customerGuestAppBuilderId: number | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    isDeleted: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerHouseKeeping.UpdateCustomerHouseKeeping {
export interface UpdateCustomerHouseKeepingIn {
    customerHouseKeepingCategories: UpdateCustomerHouseKeepingCategoryIn;
}

export interface UpdateCustomerHouseKeepingCategoryIn {
    id: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
    customerHouseKeepingItems: UpdateCustomerHouseKeepingItemIn[];
}

export interface UpdateCustomerHouseKeepingItemIn {
    id: number;
    customerGuestAppBuilderId: number | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    isDeleted: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerHouseKeeping.UpdateCustomerHouseKeeping {
export interface UpdateCustomerHouseKeepingOut extends BaseResponseOut {
    updatedCustomerHouseKeepingOut: UpdatedCustomerHouseKeepingOut;
}

export interface UpdatedCustomerHouseKeepingOut {
    id: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
}

}
export namespace CustomerMerchantAccount.GetCustomerMerchantAccount {
export interface GetCustomerMerchantAccountIn {
    customerId: number;
}

}
export namespace CustomerMerchantAccount.GetCustomerMerchantAccount {
export interface GetCustomerMerchantAccountOut extends BaseResponseOut {
    customerCommunicationOut: CustomerMerchantAccountOut;
}

export interface CustomerMerchantAccountOut {
    type: string | null;
    id: string | null;
    display_Name: string | null;
    outbound_Webhook_Url: string | null;
    outbound_Webhook_Username: string | null;
    outbound_Webhook_Password: string | null;
    visa_Network_Tokens_Requestor_Id: string | null;
    visa_Network_Tokens_App_Id: string | null;
    amex_Network_Tokens_Requestor_Id: string | null;
    amex_Network_Tokens_App_Id: string | null;
    mastercard_Network_Tokens_Requestor_Id: string | null;
    mastercard_Network_Tokens_App_Id: string | null;
    created_At: string | null;
    updated_At: string | null;
}

}
export namespace CustomerPropertyEmergencyNumbers.CreateCustomerPropertyEmergencyNumber {
export interface CreateCustomerPropertyEmergencyNumberIn {
    customerPropertyInformationId: number | null;
    name: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    isActive: boolean | null;
}

}
export namespace CustomerPropertyEmergencyNumbers.CreateCustomerPropertyEmergencyNumber {
export interface CreateCustomerPropertyEmergencyNumberOut extends BaseResponseOut {
    createdCustomerPropertyEmergencyNumberOut: CreatedCustomerPropertyEmergencyNumberOut;
}

export interface CreatedCustomerPropertyEmergencyNumberOut {
    id: number;
    customerPropertyInformationId: number | null;
    name: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    isActive: boolean | null;
}

}
export namespace CustomerPropertyEmergencyNumbers.DeleteCustomerPropertyEmergencyNumber {
export interface DeleteCustomerPropertyEmergencyNumberIn {
    id: number;
}

export interface CustomerPropertyEmergencyNumberJsonOut {
    id: number;
    customerPropertyInformationId: number | null;
    name: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    isDeleted: boolean;
}

}
export namespace CustomerPropertyEmergencyNumbers.DeleteCustomerPropertyEmergencyNumber {
export interface DeleteCustomerPropertyEmergencyNumberOut extends BaseResponseOut {
    deletedCustomerPropertyEmergencyNumberOut: DeletedCustomerPropertyEmergencyNumberOut;
}

export interface DeletedCustomerPropertyEmergencyNumberOut {
    id: number;
}

}
export namespace CustomerPropertyEmergencyNumbers.GetCustomerPropertyEmergencyNumberById {
export interface GetCustomerPropertyEmergencyNumberByIdIn {
    id: number;
}

}
export namespace CustomerPropertyEmergencyNumbers.GetCustomerPropertyEmergencyNumberById {
export interface GetCustomerPropertyEmergencyNumberByIdOut extends BaseResponseOut {
    customerPropertyEmergencyNumberByIdOut: CustomerPropertyEmergencyNumberByIdOut;
}

export interface CustomerPropertyEmergencyNumberByIdOut {
    id: number;
    customerPropertyInformationId: number | null;
    name: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
}

}
export namespace CustomerPropertyEmergencyNumbers.GetCustomerPropertyEmergencyNumbers {
export interface GetCustomerPropertyEmergencyNumbersIn {
    propertyId: number;
}

}
export namespace CustomerPropertyEmergencyNumbers.GetCustomerPropertyEmergencyNumbers {
export interface GetCustomerPropertyEmergencyNumbersOut extends BaseResponseOut {
    customerPropertyEmergencyNumbers: CustomerPropertyEmergencyNumbersOut[];
}

export interface CustomerPropertyEmergencyNumbersOut {
    id: number;
    customerPropertyInformationId: number | null;
    name: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    displayOrder: number | null;
}

}
export namespace CustomerPropertyEmergencyNumbers.UpdateCustomerPropeetyEmergencyNumberDisplayOrder {
export interface UpdatedCustomerPropeetyEmergencyNumberDisplayOrderIn {
    updateCustomerPropeetyEmergencyNumberDisplayorderIn: UpdateCustomerPropeetyEmergencyNumberDisplayOrderIn[];
}

export interface UpdateCustomerPropeetyEmergencyNumberDisplayOrderIn {
    id: number | null;
    displayOrder: number | null;
}

}
export namespace CustomerPropertyEmergencyNumbers.UpdateCustomerPropeetyEmergencyNumberDisplayOrder {
export interface UpdateCustomerPropertyEmergencyNumberDisplayOrderOuts extends BaseResponseOut {

}

}
export namespace CustomerPropertyEmergencyNumbers.UpdateCustomerPropertyEmergencyNumber {
export interface UpdateCustomerPropertyEmergencyNumberIn {
    id: number;
    customerPropertyInformationId: number | null;
    name: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    isDeleted: boolean;
    displayOrder: number | null;
}

}
export namespace CustomerPropertyEmergencyNumbers.UpdateCustomerPropertyEmergencyNumber {
export interface UpdateCustomerPropertyEmergencyNumberOut extends BaseResponseOut {
    updatedCustomerPropertyEmergencyNumberOut: UpdatedCustomerPropertyEmergencyNumberOut;
}

export interface UpdatedCustomerPropertyEmergencyNumberOut {
    id: number;
    customerPropertyInformationId: number | null;
    name: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    displayOrder: number | null;
}

}
export namespace CustomerPropertyExtras.CreateCustomerPropertyExtras {
export interface CreateCustomerPropertyExtrasIn {
    customerPropertyInformationId: number | null;
    extraType: number | null;
    name: string | null;
    description: string | null;
    link: string | null;
    isActive: boolean | null;
}

}
export namespace CustomerPropertyExtras.CreateCustomerPropertyExtras {
export interface CreateCustomerPropertyExtrasOut extends BaseResponseOut {
    createdCustomerPropertyExtrasOut: CreatedCustomerPropertyExtrasOut;
}

export interface CreatedCustomerPropertyExtrasOut {
    id: number;
}

}
export namespace CustomerPropertyExtras.DeleteCustomerPropertyExtraDetail {
export interface DeleteCustomerPropertyExtraDetailIn {
    id: number;
}

export interface CustomerPropertyExtraDetailJsonOut {
    id: number;
    description: string | null;
    link: string | null;
    isDeleted: boolean;
}

}
export namespace CustomerPropertyExtras.DeleteCustomerPropertyExtraDetail {
export interface DeleteCustomerPropertyExtraDetailOut extends BaseResponseOut {
    deletedCustomer: DeletedCustomerPropertyExtraDetailOut;
}

export interface DeletedCustomerPropertyExtraDetailOut {
    id: number;
}

}
export namespace CustomerPropertyExtras.DeleteCustomerPropertyExtras {
export interface DeleteCustomerPropertyExtrasIn {
    id: number;
}

export interface CustomerPropertyExtrasJsonOut {
    id: number;
    customerPropertyInformationId: number | null;
    extraType: number | null;
    name: string | null;
    isDeleted: boolean;
    customerPropertyExtraDetailsOuts: CustomerPropertyExtraDetailsIn[];
}

export interface CustomerPropertyExtraDetailsIn {
    id: number;
    description: string | null;
    link: string | null;
    isDeleted: boolean;
}

}
export namespace CustomerPropertyExtras.DeleteCustomerPropertyExtras {
export interface DeleteCustomerPropertyExtrasOut extends BaseResponseOut {
    deletedCustomerPropertyExtrasOut: DeletedCustomerPropertyExtrasOut;
}

export interface DeletedCustomerPropertyExtrasOut {
    id: number;
}

}
export namespace CustomerPropertyExtras.GetCustomerPropertyExtraById {
export interface GetCustomerPropertyExtraByIdIn {
    id: number;
}

}
export namespace CustomerPropertyExtras.GetCustomerPropertyExtraById {
export interface GetCustomerPropertyExtraByIdOut extends BaseResponseOut {
    customerPropertyExtraByIdOut: CustomerPropertyExtraByIdOut;
}

export interface CustomerPropertyExtraByIdOut {
    id: number;
    customerPropertyInformationId: number | null;
    extraType: number | null;
    name: string | null;
    description: string | null;
    link: string | null;
}

}
export namespace CustomerPropertyExtras.GetCustomerPropertyExtras {
export interface GetCustomerPropertyExtrasIn {
    customerPropertyInformationId: number;
}

}
export namespace CustomerPropertyExtras.GetCustomerPropertyExtras {
export interface GetCustomerPropertyExtrasOut extends BaseResponseOut {
    customerPropertyExtra: CustomerPropertyExtraOut[];
}

export interface CustomerPropertyExtraOut {
    id: number;
    customerPropertyInformationId: number | null;
    extraType: number | null;
    name: string | null;
    isDeleted: boolean;
    displayOrder: number | null;
    customerPropertyExtraDetailsOuts: CustomerPropertyExtraDetailsOut[];
}

export interface CustomerPropertyExtraDetailsOut {
    id: number;
    customerPropertyExtraId: number;
    description: string | null;
    latitude: string | null;
    longitude: string | null;
    isDeleted: boolean;
}

}
export namespace CustomerPropertyExtras.UpdateCustomerPropertyExtraDisplayOrder {
export interface UpdatedCustomerPropertyExtraDisplayOrderIn {
    updateCustomerPropertyExtraDisplayOrderIns: UpdateCustomerPropertyExtraDisplayOrderIn[];
}

export interface UpdateCustomerPropertyExtraDisplayOrderIn {
    id: number | null;
    displayOrder: number | null;
    extraType: number | null;
}

}
export namespace CustomerPropertyExtras.UpdateCustomerPropertyExtraDisplayOrder {
export interface UpdateCustomerPropertyExtraDisplayOrderOut extends BaseResponseOut {

}

}
export namespace CustomerPropertyExtras.UpdateCustomerPropertyExtras {
export interface UpdateCustomerPropertyExtrasIn {
    customerPropertyExtrasIns: CustomerPropertyExtrasIn[];
}

export interface CustomerPropertyExtrasIn {
    id: number;
    customerPropertyInformationId: number | null;
    extraType: number | null;
    name: string | null;
    isDeleted: boolean;
    displayOrder: number | null;
    customerPropertyExtraDetailsOuts: CustomerPropertyExtraDetailsIn[];
}

export interface CustomerPropertyExtraDetailsIn {
    id: number;
    description: string | null;
    latitude: string | null;
    longitude: string | null;
    isDeleted: boolean;
}

}
export namespace CustomerPropertyExtras.UpdateCustomerPropertyExtras {
export interface UpdateCustomerPropertyExtrasOut extends BaseResponseOut {

}

}
export namespace CustomerPropertyService.CreateCustomerPropertyService {
export interface CreateCustomerPropertyServiceIn {
    id: number;
    customerPropertyInformationId: number | null;
    name: string | null;
    icon: string | null;
    description: string | null;
    isActive: boolean | null;
    isDeleted: boolean;
    customerPropertyInfoServiceImagesOuts: CustomerPropertyServiceImageIn[];
}

export interface CustomerPropertyServiceImageIn {
    id: number;
    serviceImages: string | null;
    isDeleted: boolean;
}

}
export namespace CustomerPropertyService.CreateCustomerPropertyService {
export interface CreateCustomerPropertyServiceOut extends BaseResponseOut {
    customerPropertyInfoServiceImagesOuts: CreatedCustomerPropertyServiceOut;
}

export interface CreatedCustomerPropertyServiceOut {
    id: number;
    customerPropertyInformationId: number | null;
    name: string | null;
}

}
export namespace CustomerPropertyService.DeleteCustomerPropertyService {
export interface DeleteCustomerPropertyServiceIn {
    id: number;
}

export interface CustomerPropertyServiceJsonOut {
    id: number;
    customerPropertyInformationId: number;
    name: string | null;
    icon: string | null;
    description: string | null;
    isActive: boolean;
    isDeleted: boolean;
    customerPropertyInfoServiceImagesOuts: CustomerPropertyInfoServiceImagesOuts[];
}

export interface CustomerPropertyInfoServiceImagesOuts {
    id: number;
    serviceImages: string | null;
    isDeleted: boolean;
}

}
export namespace CustomerPropertyService.DeleteCustomerPropertyService {
export interface DeleteCustomerPropertyServiceOut extends BaseResponseOut {

}

}
export namespace CustomerPropertyService.GetCustomerPropertyServiceById {
export interface GetCustomerPropertyServiceByIdIn {
    id: number;
}

}
export namespace CustomerPropertyService.GetCustomerPropertyServiceById {
export interface GetCustomerPropertyServiceByIdOut extends BaseResponseOut {
    customerPropertyService: CustomerPropertyServiceByd;
}

export interface CustomerPropertyServiceByd {
    id: number;
    name: string;
    icon: number;
    description: string;
    isActive: boolean;
    serviceImage: string;
}

}
export namespace CustomerPropertyService.GetCustomerPropertyServices {
export interface GetCustomerPropertyServicesIn {
    customerPropertyInformationId: number;
}

}
export namespace CustomerPropertyService.GetCustomerPropertyServices {
export interface GetCustomerPropertyServicesOut extends BaseResponseOut {
    customerPropertyServicesOut: CustomerPropertyServicesOut[];
}

export interface CustomerPropertyServicesOut {
    id: number;
    customerPropertyInformationId: number | null;
    name: string | null;
    icon: string | null;
    description: string | null;
    isDeleted: boolean;
    customerPropertyInfoServiceImagesOuts: CustomerPropertyServiceImageOut[];
}

export interface CustomerPropertyServiceImageOut {
    id: number;
    customerPropertyServiceId: number | null;
    serviceImages: string | null;
    isDeleted: boolean;
}

}
export namespace CustomerPropertyService.UpdateCustomerPropertyService {
export interface UpdateCustomerPropertyServiceIn {
    id: number;
    customerPropertyInformationId: number | null;
    name: string | null;
    icon: string | null;
    description: string | null;
    isActive: boolean | null;
    uPCustomerPropertyServiceImageIns: UPCustomerPropertyServiceImageIn[];
}

export interface UPCustomerPropertyServiceImageIn {
    id: number;
    serviceImage: string | null;
}

}
export namespace CustomerPropertyService.UpdateCustomerPropertyService {
export interface UpdateCustomerPropertyServiceOut extends BaseResponseOut {

}

}
export namespace CustomerPropertyServiceImage.CreateCustomerPropertyServiceImage {
export interface CreateCustomerPropertyServiceImageIn {
    customerPropertyServiceId: number | null;
    isActive: boolean | null;
}

}
export namespace CustomerPropertyServiceImage.CreateCustomerPropertyServiceImage {
export interface CreateCustomerPropertyServiceImageOut extends BaseResponseOut {

}

}
export namespace CustomerPropertyServiceImage.DeleteCustomerPropertyServiceImage {
export interface DeleteCustomerPropertyServiceImageIn {
    id: number;
}

}
export namespace CustomerPropertyServiceImage.DeleteCustomerPropertyServiceImage {
export interface DeleteCustomerPropertyServiceImageOut extends BaseResponseOut {
    removeCustomerPropertyServiceImageOut: RemoveCustomerPropertyServiceImageOut;
}

export interface RemoveCustomerPropertyServiceImageOut {
    deletedCustomerPropertyServiceImageId: number;
}

}
export namespace CustomerPropertyServiceImage.GetCustomerPropertyServiceImages {
export interface GetCustomerPropertyServiceImagesIn {
    customerPropertyServiceId: number | null;
}

}
export namespace CustomerPropertyServiceImage.GetCustomerPropertyServiceImages {
export interface GetCustomerPropertyServiceImagesOut extends BaseResponseOut {
    customerPropertyServiceImages: CustomerPropertyServiceImagesOut[];
}

export interface CustomerPropertyServiceImagesOut {
    memoryStream: any | null;
    fileName: string | null;
    contentType: string | null;
}

}
export namespace CustomerReception.CreateCustomerReception {
export interface CreateCustomerReceptionIn {
    cuastomerReceptionCategories: CreateCustomerReceptionCategoryIn[];
}

export interface CreateCustomerReceptionCategoryIn {
    customerId: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    customerReceptionItems: CreateCustomerReceptionItemIn[];
}

export interface CreateCustomerReceptionItemIn {
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
}

}
export namespace CustomerReception.CreateCustomerReception {
export interface CreateCustomerReceptionOut extends BaseResponseOut {
    createdCustomerReceptionOut: CreatedCustomerReceptionOut[];
}

export interface CreatedCustomerReceptionOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
}

}
export namespace CustomerReception.DeleteCustomerReception {
export interface DeleteCustomerReceptionIn {
    id: number;
}

}
export namespace CustomerReception.DeleteCustomerReception {
export interface DeleteCustomerReceptionOut extends BaseResponseOut {
    deletedCustomerReceptionOut: DeletedCustomerReceptionOut;
}

export interface DeletedCustomerReceptionOut {
    id: number;
}

}
export namespace CustomerReception.DisplayOrderCustomerReception {
export interface DisplayOrderCustomerReceptionIn {
    displayOrderCustomerReception: DisplayOrderCustomerReception[];
}

export interface DisplayOrderCustomerReception {
    id: number | null;
    displayOrder: number | null;
}

}
export namespace CustomerReception.DisplayOrderCustomerReception {
export interface DisplayOrderCustomerReceptionOut extends BaseResponseOut {

}

export interface DisplayOrderCustomerReceptionJsonOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
    customerReceptionItems: CustomerReceptionItem[];
}

export interface CustomerReceptionItem {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    isDeleted: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerReception.GetCustomerReceptionWithRelation {
export interface GetCustomerReceptionWithRelationIn {
    appBuilderId: number;
}

}
export namespace CustomerReception.GetCustomerReceptionWithRelation {
export interface GetCustomerReceptionWithRelationOut extends BaseResponseOut {
    customerReceptionWithRelationOut: CustomerReceptionWithRelationOut[];
}

export interface CustomerReceptionWithRelationOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
    customerReceptionItems: CustomerReceptionItemsRelationOut[];
}

export interface CustomerReceptionItemsRelationOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    isDeleted: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerReception.UpdateCustomerReception {
export interface UpdateCustomerReceptionIn {
    customerReceptionCategories: UpdateCustomerReceptionCategoryIn;
}

export interface UpdateCustomerReceptionCategoryIn {
    id: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
    customerReceptionItems: UpdateCustomerReceptionItemIn[];
}

export interface UpdateCustomerReceptionItemIn {
    id: number;
    customerGuestAppBuilderId: number | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    isDeleted: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerReception.UpdateCustomerReception {
export interface UpdateCustomerReceptionOut extends BaseResponseOut {
    updatedCustomerReceptionOut: UpdatedCustomerReceptionOut;
}

export interface UpdatedCustomerReceptionOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
}

}
export namespace CustomerReservation.CreateCustomerAutoReservationWithGestDetail {
export interface CreateCustomerAutoReservationWithGestDetailIn {
    customerId: number | null;
    source: string | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    title: string | null;
    firstname: string | null;
    lastname: string | null;
    email: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    firstJourneyStep: number | null;
    roomNumber: string | null;
}

}
export namespace CustomerReservation.CreateCustomerAutoReservationWithGestDetail {
export interface CreateCustomerAutoReservationWithGestDetailOut extends BaseResponseOut {
    createdCustomerReservationOut: CreatedCustomerReservationOut;
}

export interface CreatedCustomerReservationOut {
    id: number;
    customerId: number | null;
    uuid: string | null;
    reservationNumber: string | null;
    source: string | null;
    noOfGuestAdults: number | null;
    noOfGuestChilderns: number | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    isActive: boolean | null;
}

}
export namespace CustomerReservation.CreateCustomerGuestReservation {
export interface CreateCustomerGuestReservationIn {
    reservationNumber: string;
    locationCode: string;
    userCode: string;
    title: string;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    arrivalDate: string;
    departureDate: string;
    arrivalTime: string;
    departureTime: string;
    bluetoothPinCode: string | null;
    appAccessCode: string | null;
    keyId: number | null;
}

}
export namespace CustomerReservation.CreateCustomerGuestReservation {
export interface CreateCustomerGuestReservationOut extends BaseResponseOut {
    createdCustomerReservationOut: CreatedCustomerReservationOut;
}

export interface CreatedCustomerReservationOut {
    id: number;
    customerId: number | null;
    uuid: string | null;
    reservationNumber: string | null;
    source: string | null;
    noOfGuestAdults: number | null;
    noOfGuestChilderns: number | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    isActive: boolean | null;
}

}
export namespace CustomerReservation.CreateCustomerReservation {
export interface CreateCustomerReservationIn {
    customerId: number | null;
    uuid: string | null;
    reservationNumber: string | null;
    source: string | null;
    noOfGuestAdults: number | null;
    noOfGuestChilderns: number | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    isActive: boolean | null;
}

}
export namespace CustomerReservation.CreateCustomerReservation {
export interface CreateCustomerReservationOut extends BaseResponseOut {
    createdCustomerReservationOut: CreatedCustomerReservationOut;
}

export interface CreatedCustomerReservationOut {
    id: number;
    customerId: number | null;
    uuid: string | null;
    reservationNumber: string | null;
    source: string | null;
    noOfGuestAdults: number | null;
    noOfGuestChilderns: number | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    isActive: boolean | null;
}

}
export namespace CustomerReservation.DeleteCustomerReservation {
export interface DeleteCustomerReservationIn {
    id: number;
}

}
export namespace CustomerReservation.DeleteCustomerReservation {
export interface DeleteCustomerReservationOut extends BaseResponseOut {
    deletedCustomerReservationOut: DeletedCustomerReservationOut;
}

export interface DeletedCustomerReservationOut {
    id: number;
}

}
export namespace CustomerReservation.ExtendCustomerGuestReservation {
export interface ExtendCustomerGuestReservationIn {
    reservationNumber: string;
    locationCode: string;
    userCode: string;
    departureDate: string;
    departureTime: string;
}

}
export namespace CustomerReservation.ExtendCustomerGuestReservation {
export interface ExtendCustomerGuestReservationOut extends BaseResponseOut {
    updatedCustomerReservationOut: ExtendedCustomerReservationOut;
}

export interface ExtendedCustomerReservationOut {
    id: number;
    customerId: number | null;
    uuid: string | null;
    reservationNumber: string | null;
    source: string | null;
    noOfGuestAdults: number | null;
    noOfGuestChilderns: number | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    isActive: boolean | null;
}

}
export namespace CustomerReservation.GetCustomerReservationById {
export interface GetCustomerReservationByIdIn {
    id: number;
}

}
export namespace CustomerReservation.GetCustomerReservationById {
export interface GetCustomerReservationByIdOut extends BaseResponseOut {
    customerReservationByIdOut: CustomerReservationByIdOut;
}

export interface CustomerReservationByIdOut {
    id: number;
    customerId: number | null;
    uuid: string | null;
    reservationNumber: string | null;
    source: string | null;
    noOfGuestAdults: number | null;
    noOfGuestChildrens: number | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    isActive: boolean | null;
}

}
export namespace CustomerReservation.GetCustomerReservationByReservationNumber {
export interface GetCustomerReservationByReservationNumberIn {
    reservationNumber: string;
}

}
export namespace CustomerReservation.GetCustomerReservationByReservationNumber {
export interface GetCustomerReservationByReservationNumberOut extends BaseResponseOut {
    customerReservationByNumberOut: CustomerReservationByNumberOut;
}

export interface CustomerReservationByNumberOut {
    id: number;
    customerId: number | null;
    uuid: string | null;
    reservationNumber: string | null;
    source: string | null;
    noOfGuestAdults: number | null;
    noOfGuestChildrens: number | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    isActive: boolean | null;
}

}
export namespace CustomerReservation.GetCustomerReservations {
export interface GetCustomerReservationsIn {
    customerId: number;
    searchColumn: string;
    searchValue: string;
    pageNo: number | null;
    pageSize: number | null;
    sortColumn: string | null;
    sortOrder: string | null;
}

}
export namespace CustomerReservation.GetCustomerReservations {
export interface GetCustomerReservationsOut extends BaseResponseOut {
    customerReservationsOut: CustomerReservationsOut[];
}

export interface CustomerReservationsOut {
    id: number;
    customerId: number | null;
    uuid: string | null;
    reservationNumber: string | null;
    source: string | null;
    noOfGuestAdults: number | null;
    noOfGuestChildrens: number | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    isActive: boolean | null;
}

}
export namespace CustomerReservation.TransferCustomerGuestReservation {
export interface TransferCustomerGuestReservationIn {
    reservationNumber: string;
    locationCode: string;
    userCode: string;
    newLocationCode: string;
    arrivalDate: string;
    arrivalTime: string;
    departureDate: string;
    departureTime: string;
}

}
export namespace CustomerReservation.TransferCustomerGuestReservation {
export interface TransferCustomerGuestReservationOut extends BaseResponseOut {
    updatedCustomerReservationOut: TransferedCustomerReservationOut;
}

export interface TransferedCustomerReservationOut {
    id: number;
    customerId: number | null;
    uuid: string | null;
    reservationNumber: string | null;
    source: string | null;
    noOfGuestAdults: number | null;
    noOfGuestChilderns: number | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    isActive: boolean | null;
}

}
export namespace CustomerReservation.UpdateCustomerGuestReservation {
export interface UpdateCustomerGuestReservationIn {
    reservationNumber: string;
    locationCode: string;
    userCode: string;
    title: string;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
}

}
export namespace CustomerReservation.UpdateCustomerGuestReservation {
export interface UpdateCustomerGuestReservationOut extends BaseResponseOut {
    updatedCustomerReservationOut: UpdatedCustomerReservationOut;
}

export interface UpdatedCustomerReservationOut {
    id: number;
    customerId: number | null;
    uuid: string | null;
    reservationNumber: string | null;
    source: string | null;
    noOfGuestAdults: number | null;
    noOfGuestChilderns: number | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    isActive: boolean | null;
}

}
export namespace CustomerReservation.UpdateCustomerReservation {
export interface UpdateCustomerReservationIn {
    id: number;
    customerId: number | null;
    uuid: string | null;
    reservationNumber: string | null;
    source: string | null;
    noOfGuestAdults: number | null;
    noOfGuestChilderns: number | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    isActive: boolean | null;
}

}
export namespace CustomerReservation.UpdateCustomerReservation {
export interface UpdateCustomerReservationOut extends BaseResponseOut {
    updatedCustomerReservationOut: UpdatedCustomerReservationOut;
}

export interface UpdatedCustomerReservationOut {
    id: number;
    customerId: number | null;
    uuid: string | null;
    reservationNumber: string | null;
    source: string | null;
    noOfGuestAdults: number | null;
    noOfGuestChilderns: number | null;
    checkinDate: string | null;
    checkoutDate: string | null;
    isActive: boolean | null;
}

}
export namespace CustomerRoomNames.GetCustomerRoomByCustomerRoomGuId {
export interface GetCustomerRoomByCustomerRoomGuIdIn {
    guid: string;
    customerId: number;
}

}
export namespace CustomerRoomNames.GetCustomerRoomByCustomerRoomGuId {
export interface GetCustomerRoomByCustomerRoomGuIdOut extends BaseResponseOut {
    customerRoomNamesOut: CustomerRooms;
}

export interface CustomerRooms {
    locationCode: string;
    name: string;
    streetName: string;
    streetNumber: string;
    city: string;
    state: string;
    postalCode: string;
    country: string;
    floor: string;
    apartmentNumber: string;
}

}
export namespace CustomerRoomNames.GetCustomerRoomByGuIds {
export interface GetCustomerRoomByGuIdsIn {
    guids: string[];
}

}
export namespace CustomerRoomNames.GetCustomerRoomByGuIds {
export interface GetCustomerRoomByGuIdsOut extends BaseResponseOut {
    customerRoomNamesOut: CustomerRooms[];
}

export interface CustomerRooms {
    locationCode: string;
    name: string;
    streetName: string;
    streetNumber: string;
    city: string;
    state: string;
    postalCode: string;
    country: string;
    floor: string;
    apartmentNumber: string;
}

}
export namespace CustomerRoomNames.GetCustomerRoomNames {
export interface GetCustomerRoomNamesIn {

}

}
export namespace CustomerRoomNames.GetCustomerRoomNames {
export interface GetCustomerRoomNamesOut extends BaseResponseOut {
    customerRoomNamesOut: CustomerAppBuilders[];
}

export interface CustomerAppBuilders {
    id: number;
    name: string | null;
    isWork: number | null;
    bizType: string | null;
    noOfRooms: number | null;
}

}
export namespace CustomerRoomService.CreateCustomerRoomService {
export interface CreateCustomerRoomServiceIn {
    customerRoomServiceCategories: CreateCustomerRoomServiceCategoryIn[];
}

export interface CreateCustomerRoomServiceCategoryIn {
    id: number;
    customerId: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    customerRoomServiceItems: CreateCustomerRoomServiceItemIn[];
}

export interface CreateCustomerRoomServiceItemIn {
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
}

}
export namespace CustomerRoomService.CreateCustomerRoomService {
export interface CreateCustomerRoomServiceOut extends BaseResponseOut {
    createdCustomerRoomServiceOut: CreatedCustomerRoomServiceOut[];
}

export interface CreatedCustomerRoomServiceOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    customerRoomServiceItems: CreatedCustomerRoomServiceItemOut[];
}

export interface CreatedCustomerRoomServiceItemOut {
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
}

}
export namespace CustomerRoomService.DeleteCustomerRoomService {
export interface DeleteCustomerRoomServiceIn {
    id: number;
}

}
export namespace CustomerRoomService.DeleteCustomerRoomService {
export interface DeleteCustomerRoomServiceOut extends BaseResponseOut {
    deletedCustomerRoomServiceOut: DeletedCustomerRoomServiceOut;
}

export interface DeletedCustomerRoomServiceOut {
    id: number;
}

}
export namespace CustomerRoomService.DisplayOrderCustomerRoomService {
export interface DisplayOrderCustomerRoomServiceIn {
    displayOrderCustomerRoomService: DisplayOrderCustomerRoomService[];
}

export interface DisplayOrderCustomerRoomService {
    id: number | null;
    displayOrder: number | null;
}

}
export namespace CustomerRoomService.DisplayOrderCustomerRoomService {
export interface DisplayOrderCustomerRoomServiceOut extends BaseResponseOut {

}

export interface DisplayOrderCustomerRoomServiceJsonOut {
    id: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
    customerRoomServiceItems: CustomerRoomServiceItem[];
}

export interface CustomerRoomServiceItem {
    id: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    isDeleted: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerRoomService.GetCustomerRoomServiceWithRelation {
export interface GetCustomerRoomServiceWithRelationIn {
    appBuilderId: number;
}

}
export namespace CustomerRoomService.GetCustomerRoomServiceWithRelation {
export interface GetCustomerRoomServiceWithRelationOut extends BaseResponseOut {
    customerRoomServiceWithRelationOut: CustomerRoomServiceWithRelationOut[];
}

export interface CustomerRoomServiceWithRelationOut {
    id: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
    customerRoomServiceItems: CustomerRoomServiceItemsRelationOut[];
}

export interface CustomerRoomServiceItemsRelationOut {
    id: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
}

}
export namespace CustomerRoomService.UpdateCustomerRoomService {
export interface UpdateCustomerRoomServiceIn {
    updateCustomerRoomServiceCategoryIn: UpdateCustomerRoomServiceCategoryIn;
}

export interface UpdateCustomerRoomServiceCategoryIn {
    id: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
    customerRoomServiceItems: UpdateCustomerRoomServiceItemIn[];
}

export interface UpdateCustomerRoomServiceItemIn {
    id: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    isDeleted: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomerRoomService.UpdateCustomerRoomService {
export interface UpdateCustomerRoomServiceOut extends BaseResponseOut {
    updatedCustomerRoomServiceOut: UpdatedCustomerRoomServiceOut;
}

export interface UpdatedCustomerRoomServiceOut {
    id: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    customerRoomServiceItems: UpdatedCustomerRoomServiceItemOut[];
}

export interface UpdatedCustomerRoomServiceItemOut {
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
}

}
export namespace Customers.CreateCustomer {
export interface CreateCustomerIn {
    businessName: string | null;
    businessTypeId: number | null;
    noOfRooms: number | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    isActive: boolean | null;
    productId: number | null;
    centurianHotelCode: string | null;
    watsappCountry: string | null;
    watsappNumber: string | null;
    viberCountry: string | null;
    viberNumber: string | null;
    telegramCounty: string | null;
    telegramNumber: string | null;
    vatNumber: string | null;
    customerUserIn: CustomerUserIn;
}

export interface CustomerUserIn {
    isActive: boolean | null;
    customerId: number | null;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    title: string | null;
    userName: string | null;
    password: string | null;
}

}
export namespace Customers.CreateCustomer {
export interface CreateCustomerOut extends BaseResponseOut {
    createdCustomerOut: CreatedCustomerOut;
}

export interface CreatedCustomerOut {
    customerId: number;
    userUniqueId: string | null;
    businessName: string;
}

}
export namespace Customers.CreateERPCustomer {
export interface CreateERPCustomerIn {
    pylonUniqueCustomerId: string | null;
    firstName: string | null;
    lastName: string | null;
    companyName: string | null;
    businessType: string | null;
    noOfRooms: number | null;
    phoneCountry: string | null;
    mobile: string | null;
    servicePack: string | null;
    expirationInDay: number;
    email: string | null;
    username: string | null;
    password: string | null;
}

}
export namespace Customers.CreateERPCustomer {
export interface CreateERPCustomerOut extends BaseResponseOut {
    createdCustomerOut: CreatedERPCustomerOut;
}

export interface CreatedERPCustomerOut {
    pylonUniqueCustomerId: string | null;
}

}
export namespace Customers.DeleteCustomer {
export interface DeleteCustomerIn {
    id: number;
}

}
export namespace Customers.DeleteCustomer {
export interface DeleteCustomerOut extends BaseResponseOut {
    deleteCustomerClass: DeleteCustomerClass;
}

export interface DeleteCustomerClass {
    id: number;
}

}
export namespace Customers.ERPCustomerActivation {
export interface ERPCustomerActivationIn {
    pylonUniqueCustomerId: string | null;
    customerStatus: string | null;
}

}
export namespace Customers.ERPCustomerActivation {
export interface ERPCustomerActivationOut extends BaseResponseOut {
    eRPCustomerStatusOutput: ERPCustomerstatusOut;
}

export interface ERPCustomerstatusOut {
    pylonUniqueCustomerId: string | null;
    customerStatus: boolean | null;
}

}
export namespace Customers.GetCustomerByGuId {
export interface GetCustomerByGuIdIn {
    guId: string;
}

}
export namespace Customers.GetCustomerByGuId {
export interface GetCustomerByGuIdOut extends BaseResponseOut {
    customerByIdForHospitioOut: CustomerByGuIdOut;
}

export interface CustomerByGuIdOut {
    id: number;
    businessName: string | null;
    businessTypeId: number | null;
    noOfRooms: number | null;
    timeZone: string | null;
    whatsappCountry: string | null;
    whatsappNumber: string | null;
    cname: string | null;
    clientDoamin: string | null;
    messenger: string | null;
    email: string | null;
    viberCountry: string | null;
    viberNumber: string | null;
    telegramCounty: string | null;
    telegramNumber: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    businessStartTime: string | null;
    businessCloseTime: string | null;
    doNotDisturbGuestStartTime: string | null;
    doNotDisturbGuestEndTime: string | null;
    staffAlertsOffduty: boolean | null;
    noMessageToGuestWhileQuiteTime: boolean | null;
    incomingTranslationLangage: string | null;
    noTranslateWords: string | null;
    productId: number | null;
    isActive: boolean | null;
    smsTitle: string | null;
    guid: string | null;
}

}
export namespace Customers.GetCustomerById {
export interface GetCustomerByIdIn {
    id: number;
}

}
export namespace Customers.GetCustomerById {
export interface GetCustomerByIdOut extends BaseResponseOut {
    customerByIdOut: CustomerByIdOut[];
}

export interface CustomerByIdOut {
    id: number;
    businessName: string | null;
    businessTypeId: number | null;
    noOfRooms: number | null;
    timeZone: string | null;
    whatsappCountry: string | null;
    whatsappNumber: string | null;
    cname: string | null;
    clientDoamin: string | null;
    email: string | null;
    messenger: string | null;
    smsTitle: string | null;
    viberCountry: string | null;
    viberNumber: string | null;
    telegramCounty: string | null;
    telegramNumber: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    businessStartTime: string | null;
    businessCloseTime: string | null;
    doNotDisturbGuestStartTime: string | null;
    doNotDisturbGuestEndTime: string | null;
    staffAlertsOffduty: boolean | null;
    noMessageToGuestWhileQuiteTime: boolean | null;
    incomingTranslationLangage: string | null;
    noTranslateWords: string | null;
    servicePackageId: number | null;
    isTwoWayComunication: boolean | null;
    latitude: string | null;
    longitude: string | null;
    isActive: boolean | null;
    currencyCode: string | null;
    pMSId: number | null;
    pMSAPIAuthUsername: string | null;
    pMSAPIAuthPassword: string | null;
    userType: number | null;
    checkInPolicy: string | null;
    checkOutPolicy: string | null;
    embededEmail: string | null;
    updateCustomerRoomNamesOuts: UpdateCustomerRoomNamesOut[];
}

export interface UpdateCustomerRoomNamesOut {
    id: number | null;
    customerId: number | null;
    name: string | null;
    createdFrom: number | null;
    isActive: boolean | null;
    centurionLocationCode: string | null;
    locationType: number | null;
    gui: string | null;
}

}
export namespace Customers.GetCustomerByIdForHospitio {
export interface GetCustomerByIdForHospitioIn {
    id: number;
}

}
export namespace Customers.GetCustomerByIdForHospitio {
export interface GetCustomerByIdForHospitioOut extends BaseResponseOut {
    customerByIdForHospitioOut: CustomerByIdForHospitioOut;
}

export interface CustomerByIdForHospitioOut {
    id: number;
    userUniqueID: string | null;
    pMSId: number | null;
    pMSAPIAuthUsername: string | null;
    pMSAPIAuthPassword: string | null;
    businessName: string | null;
    businessTypeId: number | null;
    noOfRooms: number | null;
    currencyCode: string | null;
    timeZone: string | null;
    whatsappCountry: string | null;
    whatsappNumber: string | null;
    cname: string | null;
    clientDoamin: string | null;
    email: string | null;
    messenger: string | null;
    viberCountry: string | null;
    viberNumber: string | null;
    telegramCounty: string | null;
    telegramNumber: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    businessStartTime: string | null;
    businessCloseTime: string | null;
    doNotDisturbGuestStartTime: string | null;
    doNotDisturbGuestEndTime: string | null;
    staffAlertsOffduty: boolean | null;
    noMessageToGuestWhileQuiteTime: boolean | null;
    incomingTranslationLangage: string | null;
    noTranslateWords: string | null;
    productId: number | null;
    isActive: boolean | null;
    country: string | null;
    firstName: string | null;
    lastName: string | null;
    title: string | null;
    profilePicture: string | null;
    isTwoWayComunication: boolean | null;
    latitude: string | null;
    longitude: string | null;
    userName: string | null;
    smsTitle: string | null;
    productName: string | null;
    customerUserEmail: string | null;
    checkInPolicy: string | null;
    checkOutPolicy: string | null;
    embededEmail: string | null;
    subscriptionExpirationDate: string | null;
}

}
export namespace Customers.GetCustomerCurrency {
export interface GetCustomerCurrencyIn {
    id: number;
}

}
export namespace Customers.GetCustomerCurrency {
export interface GetCustomerCurrencyOut extends BaseResponseOut {
    customerByIdOut: CustomerCurrencyByIdOut;
}

export interface CustomerCurrencyByIdOut {
    currencyCode: string | null;
}

}
export namespace Customers.GetCustomerLatitudeLongitude {
export interface GetCustomerLatitudeLongitudeIn {
    customerId: number | null;
    builderId: number | null;
}

}
export namespace Customers.GetCustomerLatitudeLongitude {
export interface GetCustomerLatitudeLongitudeOut extends BaseResponseOut {
    customerLatitudeLongitude: CustomerLatitudeLongitude;
}

export interface CustomerLatitudeLongitude {
    businessName: string | null;
    latitude: string | null;
    longitude: string | null;
}

}
export namespace Customers.GetCustomerName {
export interface GetCustomerNameIn {
    id: number;
}

}
export namespace Customers.GetCustomerName {
export interface GetCustomerNameOut extends BaseResponseOut {
    customerNameOut: GetCustomerNameClass;
}

export interface GetCustomerNameClass {
    businessName: string | null;
}

}
export namespace Customers.GetCustomers {
export interface GetCustomersIn {

}

}
export namespace Customers.GetCustomers {
export interface GetCustomersOut extends BaseResponseOut {
    customers: CustomerDetails[];
}

export interface CustomerDetails {
    id: number;
    customerName: string;
}

}
export namespace Customers.GetCustomersByGuIds {
export interface GetCustomersByGuIdsIn {
    guids: string[];
}

}
export namespace Customers.GetCustomersByGuIds {
export interface GetCustomersByGuIdsOut extends BaseResponseOut {
    getCustomers: Customers[];
}

export interface Customers {
    userCode: string;
    username: string;
    firstname: string;
    lastname: string;
    email: string;
    mobile: string;
}

}
export namespace Customers.GetCustomersMainInfo {
export interface GetCustomersMainInfoIn {
    searchValue: string;
    pageNo: number | null;
    pageSize: number | null;
    sortColumn: string | null;
    sortOrder: string | null;
    alphabetsStartsWith: string | null;
}

}
export namespace Customers.GetCustomersMainInfo {
export interface GetCustomersMainInfoOut extends BaseResponseOut {
    customersMainInfoOut: CustomersMainInfoOut[];
}

export interface CustomersMainInfoOut {
    id: number;
    businessName: string | null;
    servicePackName: string | null;
    firstName: string | null;
    lastName: string | null;
    bizType: string | null;
    title: string | null;
    profilePicture: string | null;
    filteredCount: number | null;
}

}
export namespace Customers.GetGuestDefaultCheckinDetails {
export interface GetGuestDefaultCheckinDetailsIn {
    customerId: number;
}

}
export namespace Customers.GetGuestDefaultCheckinDetails {
export interface GetGuestDefaultCheckinDetailsOut extends BaseResponseOut {
    getGuestDefaultCheckinDetailsResponseOut: GetGuestDefaultCheckinDetailsResponseOut;
}

export interface GetGuestDefaultCheckinDetailsResponseOut {
    checkInPolicy: string | null;
    checkOutPolicy: string | null;
}

}
export namespace Customers.GetLanguages {
export interface GetLanguagesOut extends BaseResponseOut {
    languagesOut: LanguagesOut[];
}

export interface LanguagesOut {
    languageCode: string;
    nativeName: string;
    name: string;
    dir: string;
}

}
export namespace Customers.GetLanguageTranslation {
export interface GetLanguageTranslationIn {
    channelId: number;
    message: string;
}

}
export namespace Customers.GetLanguageTranslation {
export interface GetLanguageTranslationOut extends BaseResponseOut {
    languageTranslationOut: LanguageTranslationOut;
}

export interface LanguageTranslationOut {
    detectedLanguageCode: string;
    convertedMessage: string;
    convertedLanguageCode: string;
}

}
export namespace Customers.UpdateCustomer {
export interface UpdateCustomerIn {
    id: number;
    businessName: string | null;
    businessTypeId: number | null;
    noOfRooms: number | null;
    timeZone: string | null;
    whatsappCountry: string | null;
    whatsappNumber: string | null;
    cname: string | null;
    clientDoamin: string | null;
    email: string | null;
    messenger: string | null;
    smsTitle: string | null;
    viberCountry: string | null;
    viberNumber: string | null;
    telegramCounty: string | null;
    telegramNumber: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    businessStartTime: string | null;
    businessCloseTime: string | null;
    doNotDisturbGuestStartTime: string | null;
    doNotDisturbGuestEndTime: string | null;
    staffAlertsOffduty: boolean | null;
    noMessageToGuestWhileQuiteTime: boolean | null;
    incomingTranslationLangage: string | null;
    noTranslateWords: string | null;
    currencyCode: string | null;
    latitude: string | null;
    longitude: string | null;
    isActive: boolean | null;
    isTwoWayComunication: boolean | null;
    pMSId: number | null;
    pMSAPIAuthUsername: string | null;
    pMSAPIAuthPassword: string | null;
    checkInPolicy: string | null;
    checkOutPolicy: string | null;
    embededEmail: string | null;
    updateCustomerRoomNamesIns: UpdateCustomerRoomNamesIn[];
}

export interface UpdateCustomerRoomNamesIn {
    id: number;
    name: string | null;
    locationType: number | null;
    centurionLocationCode: string | null;
    isActive: boolean | null;
    gui: string | null;
}

}
export namespace Customers.UpdateCustomer {
export interface UpdateCustomerOut extends BaseResponseOut {
    updatedCustomerOut: UpdatedCustomerOut;
}

export interface UpdatedCustomerOut {
    customerId: number;
}

}
export namespace Customers.UpdateCustomerProduct {
export interface UpdateCustomerProductIn {
    id: number;
    productId: number;
}

}
export namespace Customers.UpdateCustomerProduct {
export interface UpdateCustomerProductOut extends BaseResponseOut {
    updatedCustomerProductOut: UpdatedCustomerProductOut;
}

export interface UpdatedCustomerProductOut {
    customerId: number;
}

}
export namespace Customers.UpdateCustomerUser {
export interface UpdateCustomerUserIn {
    customerId: number;
    userName: string;
    firstName: string | null;
    lastName: string | null;
    emailAddress: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    country: string | null;
    incomingTranslationLangage: string | null;
    password: string | null;
    isActive: boolean | null;
    subscriptionExpirationDate: string | null;
}

}
export namespace Customers.UpdateCustomerUser {
export interface UpdateCustomerUserOut extends BaseResponseOut {
    createdCustomerOut: UpdatedCustomerOut;
}

export interface UpdatedCustomerOut {
    customerId: number;
}

}
export namespace Customers.UpdateERPServicePack {
export interface UpdateERPServicePackIn {
    pylonUniqueCustomerId: string | null;
    purchaseType: string | null;
    servicePack: string | null;
    expirationInDay: number;
}

}
export namespace Customers.UpdateERPServicePack {
export interface UpdateERPServicePackOut extends BaseResponseOut {
    updateERPServiceOut: UpdatedERPServicePackOut;
}

export interface UpdatedERPServicePackOut {
    pylonUniqueCustomerId: string | null;
}

}
export namespace CustomersConcierge.CreateCustomerConcierge {
export interface CreateCustomerConciergeIn {
    customerConciergeCategories: CreateCustomerConciergeCategoryIn[];
}

export interface CreateCustomerConciergeCategoryIn {
    customerId: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    customerConciergeItems: CreateCustomerConciergeItemIn[];
}

export interface CreateCustomerConciergeItemIn {
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
}

}
export namespace CustomersConcierge.CreateCustomerConcierge {
export interface CreateCustomerConciergeOut extends BaseResponseOut {
    createdCustomerConciergeOut: CreatedCustomerConciergeOut[];
}

export interface CreatedCustomerConciergeOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
}

}
export namespace CustomersConcierge.DeleteCustomerConcierge {
export interface DeleteCustomerConciergeIn {
    id: number;
}

}
export namespace CustomersConcierge.DeleteCustomerConcierge {
export interface DeleteCustomerConciergeOut extends BaseResponseOut {
    deletedCustomerConciergeOut: DeletedCustomerConciergeOut;
}

export interface DeletedCustomerConciergeOut {
    id: number;
}

export interface DeleteCustomerConciergeJsonOut {
    id: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
    customerConciergeItems: DeleteCustomerConciergeItem[];
}

export interface DeleteCustomerConciergeItem {
    id: number;
    customerGuestAppBuilderId: number | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    isDeleted: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomersConcierge.DeleteCustomerConciergeItem {
export interface DeleteCustomerConciergeItemIn {
    id: number;
}

}
export namespace CustomersConcierge.DeleteCustomerConciergeItem {
export interface DeleteCustomerConciergeItemOut extends BaseResponseOut {
    deletedCustomerConciergeOut: DeletedCustomerConciergeItemOut;
}

export interface DeletedCustomerConciergeItemOut {
    id: number;
}

export interface DeleteCustomerConciergeItem {
    id: number;
    customerGuestAppBuilderId: number | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    isDeleted: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomersConcierge.DisplayOrderCustomerConcierage {
export interface DisplayOrderCustomerConcierageIn {
    displayOrderCustomerConcierage: DisplayOrderCustomerConcierage[];
}

export interface DisplayOrderCustomerConcierage {
    id: number | null;
    displayOrder: number | null;
}

}
export namespace CustomersConcierge.DisplayOrderCustomerConcierage {
export interface DisplayOrderCustomerConcierageOut extends BaseResponseOut {

}

export interface DisplayOrderCustomerConcierageJsonOut {
    id: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
    customerConciergeItems: CustomerConciergeItem[];
}

export interface CustomerConciergeItem {
    id: number;
    customerGuestAppBuilderId: number | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    isDeleted: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomersConcierge.GetCustomerConciergeWithRelation {
export interface GetCustomerConciergeWithRelationIn {
    appBuilderId: number;
}

}
export namespace CustomersConcierge.GetCustomerConciergeWithRelation {
export interface GetCustomerConciergeWithRelationOut extends BaseResponseOut {
    customerConciergeWithRelationOut: CustomerConciergeWithRelationOut[];
}

export interface CustomerConciergeWithRelationOut {
    id: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
    customerConciergeItems: CustomerConciergeItemsRelationOut[];
}

export interface CustomerConciergeItemsRelationOut {
    id: number;
    customerGuestAppBuilderId: number | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
}

}
export namespace CustomersConcierge.UpdateCustomerConcierge {
export interface UpdateCustomerConciergeIn {
    customerConciergeCategories: UpdateCustomerConciergeCategoryIn;
}

export interface UpdateCustomerConciergeCategoryIn {
    id: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    isActive: boolean | null;
    isDeleted: boolean | null;
    customerConciergeItems: UpdateCustomerConciergeItemIn[];
}

export interface UpdateCustomerConciergeItemIn {
    id: number;
    customerGuestAppBuilderId: number | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    isDeleted: boolean | null;
    isActive: boolean | null;
}

}
export namespace CustomersConcierge.UpdateCustomerConcierge {
export interface UpdateCustomerConciergeOut extends BaseResponseOut {
    updatedCustomerConciergeOut: UpdatedCustomerConciergeOut;
}

export interface UpdatedCustomerConciergeOut {
    id: number;
    customerGuestAppBuilderId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
}

}
export namespace CustomersDigitalAssistants.CreateCustomersDigitalAssistants {
export interface CreateCustomersDigitalAssistantsIn {
    customerId: number | null;
    name: string | null;
    details: string | null;
    icon: string | null;
}

}
export namespace CustomersDigitalAssistants.CreateCustomersDigitalAssistants {
export interface CreateCustomersDigitalAssistantsOut extends BaseResponseOut {
    createdCustomersDigitalAssistantsOut: CreatedCustomersDigitalAssistantsOut;
}

export interface CreatedCustomersDigitalAssistantsOut {
    id: number;
    customerId: number | null;
    name: string | null;
    details: string | null;
    icon: string | null;
}

}
export namespace CustomersDigitalAssistants.DeleteCustomersDigitalAssistants {
export interface DeleteCustomersDigitalAssistantsIn {
    id: number;
}

}
export namespace CustomersDigitalAssistants.DeleteCustomersDigitalAssistants {
export interface DeleteCustomersDigitalAssistantsOut extends BaseResponseOut {
    deletedCustomersPaymentProcessorsOut: DeletedCustomersDigitalAssistantsOut;
}

export interface DeletedCustomersDigitalAssistantsOut {
    id: number;
}

}
export namespace CustomersDigitalAssistants.GetCustomersDigitalAssistants {
export interface GetCustomersDigitalAssistantsIn {
    customerId: number | null;
}

}
export namespace CustomersDigitalAssistants.GetCustomersDigitalAssistants {
export interface GetCustomersDigitalAssistantsOut extends BaseResponseOut {
    customersDigitalAssistantsOut: CustomersDigitalAssistantsOut[];
}

export interface CustomersDigitalAssistantsOut {
    id: number;
    customerId: number | null;
    name: string | null;
    details: string | null;
    icon: string | null;
    isActive: boolean | null;
}

}
export namespace CustomersDigitalAssistants.GetCustomersDigitalAssistantsById {
export interface GetCustomersDigitalAssistantsByIdIn {
    id: number;
}

}
export namespace CustomersDigitalAssistants.GetCustomersDigitalAssistantsById {
export interface GetCustomersDigitalAssistantsByIdOut extends BaseResponseOut {
    customersDigitalAssistantsByIdOut: CustomersDigitalAssistantsByIdOut;
}

export interface CustomersDigitalAssistantsByIdOut {
    id: number;
    customerId: number | null;
    name: string | null;
    details: string | null;
    icon: string | null;
}

}
export namespace CustomersDigitalAssistants.UpdateCustomersDigitalAssistants {
export interface UpdateCustomersDigitalAssistantsIn {
    id: number;
    name: string | null;
    details: string | null;
    icon: string | null;
}

}
export namespace CustomersDigitalAssistants.UpdateCustomersDigitalAssistants {
export interface UpdateCustomersDigitalAssistantsOut extends BaseResponseOut {
    updatedCustomersPaymentProcessorsOut: UpdatedCustomersDigitalAssistantsOut;
}

export interface UpdatedCustomersDigitalAssistantsOut {
    id: number;
    name: string | null;
    details: string | null;
    icon: string | null;
}

}
export namespace CustomersDigitalAssistants.UpdateIsActiveCustomersDigitalAssistants {
export interface UpdateIsActiveCustomersDigitalAssistantsIn {
    id: number;
    isActive: boolean | null;
}

}
export namespace CustomersDigitalAssistants.UpdateIsActiveCustomersDigitalAssistants {
export interface UpdateIsActiveCustomersDigitalAssistantsOut extends BaseResponseOut {
    updatedIsActiveCustomersDigitalAssistantsOut: UpdatedIsActiveCustomersDigitalAssistantsOut;
}

export interface UpdatedIsActiveCustomersDigitalAssistantsOut {
    id: number;
    isActive: boolean | null;
}

}
export namespace CustomersGuestJourneys.CreateCustomersGuestJourneys {
export interface CreateCustomersGuestJourneysIn {
    customerId: number;
    journeyStep: number | null;
    name: string | null;
    sendType: number | null;
    timingOption1: number | null;
    timingOption2: number | null;
    timingOption3: number | null;
    timing: number | null;
    notificationDays: string | null;
    notificationTime: string | null;
    guestJourneyMessagesTemplateId: number | null;
    templetMessage: string | null;
    buttons: Button[] | null;
}

export interface Button {
    type: string | null;
    text: string | null;
    value: string | null;
}

}
export namespace CustomersGuestJourneys.CreateCustomersGuestJourneys {
export interface CreateCustomersGuestJourneysOut extends BaseResponseOut {
    createdCustomersGuestJourneysOut: CreatedCustomersGuestJourneysOut;
}

export interface CreatedCustomersGuestJourneysOut {
    id: number;
    customerId: number;
    journeyStep: number | null;
    name: string | null;
    whatsappTemplateName: string | null;
    sendType: number | null;
    timingOption1: number | null;
    timingOption2: number | null;
    timingOption3: number | null;
    timing: number | null;
    notificationDays: string | null;
    notificationTime: string | null;
    guestJourneyMessagesTemplateId: number | null;
    templetMessage: string | null;
    vonageTemplateId: string | null;
    vonageTemplateStatus: string | null;
    isActive: boolean | null;
    buttons: string | null;
}

}
export namespace CustomersGuestJourneys.DeleteCustomersGuestJourneys {
export interface DeleteCustomersGuestJourneysIn {
    id: number;
}

}
export namespace CustomersGuestJourneys.DeleteCustomersGuestJourneys {
export interface DeleteCustomersGuestJourneysOut extends BaseResponseOut {
    deletedCustomersDigitalAssistantsOut: DeletedCustomersGuestJourneysOut;
}

export interface DeletedCustomersGuestJourneysOut {
    id: number;
}

}
export namespace CustomersGuestJourneys.GetCustomersGuestJourneys {
export interface GetCustomersGuestJourneysIn {
    customerId: number;
}

}
export namespace CustomersGuestJourneys.GetCustomersGuestJourneys {
export interface GetCustomersGuestJourneysOut extends BaseResponseOut {
    customersGuestJourneysOut: CustomersGuestJourneysOut[];
}

export interface CustomersGuestJourneysOut {
    id: number;
    cutomerId: number;
    journeyStep: number | null;
    name: string | null;
    sendType: number | null;
    timingOption1: number | null;
    timingOption2: number | null;
    timingOption3: number | null;
    timing: number | null;
    notificationDays: string | null;
    notificationTime: string | null;
    guestJourneyMessagesTemplateId: number | null;
    templetMessage: string | null;
    buttons: string | null;
    vonageTemplateId: string | null;
    vonageTemplateStatus: string | null;
    isActive: boolean | null;
}

}
export namespace CustomersGuestJourneys.GetCustomersGuestJourneysById {
export interface GetCustomersGuestJourneysByIdIn {
    id: number;
}

}
export namespace CustomersGuestJourneys.GetCustomersGuestJourneysById {
export interface GetCustomersGuestJourneysByIdOut extends BaseResponseOut {
    customersGuestJourneysByIdOut: CustomersGuestJourneysByIdOut;
}

export interface CustomersGuestJourneysByIdOut {
    id: number;
    cutomerId: number;
    journeyStep: number | null;
    name: string | null;
    sendType: number | null;
    timingOption1: number | null;
    timingOption2: number | null;
    timingOption3: number | null;
    timing: number | null;
    notificationDays: string | null;
    notificationTime: string | null;
    guestJourneyMessagesTemplateId: number | null;
    templetMessage: string | null;
    buttons: string | null;
    vonageTemplateId: string | null;
    vonageTemplateStatus: string | null;
}

}
export namespace CustomersGuestJourneys.UpdateCustomersGuestJourneys {
export interface UpdateCustomersGuestJourneysIn {
    id: number;
    journeyStep: number | null;
    name: string | null;
    sendType: number | null;
    timingOption1: number | null;
    timingOption2: number | null;
    timingOption3: number | null;
    timing: number | null;
    notificationDays: string | null;
    notificationTime: string | null;
    guestJourneyMessagesTemplateId: number | null;
    templetMessage: string | null;
    buttons: Button[] | null;
}

export interface Button {
    type: string | null;
    text: string | null;
    value: string | null;
}

}
export namespace CustomersGuestJourneys.UpdateCustomersGuestJourneys {
export interface UpdateCustomersGuestJourneysOut extends BaseResponseOut {
    updatedCustomersGuestJourneysOut: UpdatedCustomersGuestJourneysOut;
}

export interface UpdatedCustomersGuestJourneysOut {
    id: number;
    journeyStep: number | null;
    name: string | null;
    whatsappTemplateName: string | null;
    sendType: number | null;
    timingOption1: number | null;
    timingOption2: number | null;
    timingOption3: number | null;
    timing: number | null;
    notificationDays: string | null;
    notificationTime: string | null;
    guestJourneyMessagesTemplateId: number | null;
    templetMessage: string | null;
    vonageTemplateId: string | null;
    vonageTemplateStatus: string | null;
    buttons: string | null;
}

}
export namespace CustomersGuestJourneys.UpdateIsActiveCustomersGuestJourneys {
export interface UpdateIsActiveCustomersGuestJourneyIn {
    id: number;
    isActive: boolean | null;
}

}
export namespace CustomersGuestJourneys.UpdateIsActiveCustomersGuestJourneys {
export interface UpdateIsActiveCustomersGuestJourneysOut extends BaseResponseOut {
    updatedIsActiveCustomersGuestJourneysOut: UpdatedIsActiveCustomersGuestJourneysOut;
}

export interface UpdatedIsActiveCustomersGuestJourneysOut {
    id: number;
    isActive: boolean | null;
}

}
export namespace CustomersPaymentProcessors.CreateCustomersPaymentProcessors {
export interface CreateCustomersPaymentProcessorsIn {
    customerId: number | null;
    paymentProcessorId: number | null;
    clientId: string | null;
    clientSecret: string | null;
    currency: string | null;
}

}
export namespace CustomersPaymentProcessors.CreateCustomersPaymentProcessors {
export interface CreateCustomersPaymentProcessorsOut extends BaseResponseOut {
    createdCustomersPaymentProcessorsOut: CreatedCustomersPaymentProcessorsOut;
}

export interface CreatedCustomersPaymentProcessorsOut {
    id: number;
    customerId: number | null;
    paymentProcessorId: number | null;
    clientId: string | null;
    clientSecret: string | null;
    currency: string | null;
}

}
export namespace CustomersPaymentProcessors.DeleteCustomersPaymentProcessors {
export interface DeleteCustomersPaymentProcessorsIn {
    id: number;
}

}
export namespace CustomersPaymentProcessors.DeleteCustomersPaymentProcessors {
export interface DeleteCustomersPaymentProcessorsOut extends BaseResponseOut {
    deletedCustomersPaymentProcessorsOut: DeletedCustomersPaymentProcessorsOut;
}

export interface DeletedCustomersPaymentProcessorsOut {
    id: number;
}

}
export namespace CustomersPaymentProcessors.GetCustomersPaymentProcessors {
export interface GetCustomersPaymentProcessorsIn {
    customerId: number;
    pageNo: number;
    pageSize: number;
}

}
export namespace CustomersPaymentProcessors.GetCustomersPaymentProcessors {
export interface GetCustomersPaymentProcessorsOut extends BaseResponseOut {
    customersPaymentProcessorsOut: CustomersPaymentProcessorsOut[];
}

export interface CustomersPaymentProcessorsOut {
    id: number;
    customerId: number | null;
    paymentProcessorId: number | null;
    clientId: string | null;
    clientSecret: string | null;
    currency: string | null;
}

}
export namespace CustomersPaymentProcessors.GetCustomersPaymentProcessorsByCustomerId {
export interface GetCustomersPaymentProcessorsByCustomerIdIn {
    customerId: number;
}

}
export namespace CustomersPaymentProcessors.GetCustomersPaymentProcessorsByCustomerId {
export interface GetCustomersPaymentProcessorsByCustomerIdOut extends BaseResponseOut {
    customersPaymentProcessorsByCustomerIdOut: CustomersPaymentProcessorsByCustomerIdOut[];
}

export interface CustomersPaymentProcessorsByCustomerIdOut {
    id: number;
    customerId: number;
    paymentProcessorId: number | null;
    gRPaymentServiceId: string | null;
    isActive: boolean | null;
    gRCategory: string | null;
    gRGroup: string | null;
    gRID: string | null;
    gRIcon: string | null;
    gRName: string | null;
}

}
export namespace CustomersPaymentProcessors.GetCustomersPaymentProcessorsById {
export interface GetCustomersPaymentProcessorsByIdIn {
    id: number;
}

}
export namespace CustomersPaymentProcessors.GetCustomersPaymentProcessorsById {
export interface GetCustomersPaymentProcessorsByIdOut extends BaseResponseOut {
    customersPaymentProcessorsByIdOut: CustomersPaymentProcessorsByIdOut;
}

export interface CustomersPaymentProcessorsByIdOut {
    id: number;
    customerId: number | null;
    paymentProcessorId: number | null;
    gRPaymentServiceId: string | null;
    gRWebhookURL: string | null;
    isActive: boolean | null;
    gR3DSecureEnabled: boolean | null;
    gRAcceptedCountries: string | null;
    gRAcceptedCurrencies: string | null;
    gRFields: string | null;
    gRIsDeleted: boolean | null;
    gRMerchantProfile: string | null;
    paymentProcessors: PaymentProcessors[] | null;
}

export interface PaymentProcessors {
    id: number;
    isActive: boolean | null;
    gRCategory: string | null;
    gRGroup: string | null;
    gRID: string | null;
    gRIcon: string | null;
    gRName: string | null;
    paymentProcessorsDefinations: PaymentProcessorsDefinations[] | null;
}

export interface PaymentProcessorsDefinations {
    id: number;
    gRFields: string | null;
    gRSupportedCountries: string | null;
    gRSupportedCurrencies: string | null;
    gRSupportedFeatures: string | null;
    paymentProcessorId: number;
    isActive: boolean | null;
}

}
export namespace CustomersPaymentProcessors.UpdateCustomersPaymentProcessors {
export interface UpdateCustomersPaymentProcessorsIn {
    id: number;
    customerId: number | null;
    paymentProcessorId: number | null;
    clientId: string | null;
    clientSecret: string | null;
    currency: string | null;
}

}
export namespace CustomersPaymentProcessors.UpdateCustomersPaymentProcessors {
export interface UpdateCustomersPaymentProcessorsOut extends BaseResponseOut {
    updatedCustomersPaymentProcessorsOut: UpdatedCustomersPaymentProcessorsOut;
}

export interface UpdatedCustomersPaymentProcessorsOut {
    id: number;
    customerId: number | null;
    paymentProcessorId: number | null;
    clientId: string | null;
    clientSecret: string | null;
    currency: string | null;
}

}
export namespace CustomersPropertiesInfo.CreateCustomersPropertiesInfo {
export interface CreateCustomersPropertiesInfoIn {
    customerId: number;
    customerGuestAppBuilderId: number | null;
    wifiUsername: string | null;
    wifiPassword: string | null;
    overview: string | null;
    checkInPolicy: string | null;
    termsAndConditions: string | null;
    street: string | null;
    streetNumber: string | null;
    city: string | null;
    postalcode: string | null;
    country: string | null;
    isActive: boolean | null;
}

}
export namespace CustomersPropertiesInfo.CreateCustomersPropertiesInfo {
export interface CreateCustomersPropertiesInfoOut extends BaseResponseOut {
    createdCustomersPropertiesInfoOut: CreatedCustomersPropertiesInfoOut;
}

export interface CreatedCustomersPropertiesInfoOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    wifiUsername: string | null;
    wifiPassword: string | null;
    overview: string | null;
    checkInPolicy: string | null;
    termsAndConditions: string | null;
    street: string | null;
    streetNumber: string | null;
    city: string | null;
    postalcode: string | null;
    country: string | null;
    isActive: boolean | null;
}

}
export namespace CustomersPropertiesInfo.DeleteCustomersPropertiesInfo {
export interface DeleteCustomersPropertiesInfoIn {
    id: number;
}

}
export namespace CustomersPropertiesInfo.DeleteCustomersPropertiesInfo {
export interface DeleteCustomersPropertiesInfoOut extends BaseResponseOut {
    deletedCustomersPropertiesInfoOut: DeletedCustomersPropertiesInfoOut;
}

export interface DeletedCustomersPropertiesInfoOut {
    id: number;
}

}
export namespace CustomersPropertiesInfo.GetCustomersPropertiesInfo {
export interface GetCustomersPropertiesInfoIn {
    searchColumn: string | null;
    searchValue: string | null;
    pageNo: number;
    pageSize: number;
    sortColumn: string | null;
    sortOrder: string | null;
    customerId: number;
}

}
export namespace CustomersPropertiesInfo.GetCustomersPropertiesInfo {
export interface GetCustomersPropertiesInfoOut extends BaseResponseOut {
    customersPropertiesInfoOut: CustomersPropertiesInfoOut[];
}

export interface CustomersPropertiesInfoOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    wifiUsername: string | null;
    wifiPassword: string | null;
    overview: string | null;
    checkInPolicy: string | null;
    termsAndConditions: string | null;
    street: string | null;
    streetNumber: string | null;
    city: string | null;
    postalcode: string | null;
    country: string | null;
    isActive: boolean | null;
    filteredCount: number | null;
}

}
export namespace CustomersPropertiesInfo.GetCustomersPropertiesInfoByAppBuilderId {
export interface GetCustomersPropertiesInfoByAppBuilderIdIn {
    appBuilderId: number;
    customerId: number;
}

}
export namespace CustomersPropertiesInfo.GetCustomersPropertiesInfoByAppBuilderId {
export interface GetCustomersPropertiesInfoByAppBuilderIdOut extends BaseResponseOut {
    customersPropertiesInfoByAppBuilderIdOut: CustomersPropertiesInfoByAppBuilderIdOut[];
}

export interface CustomersPropertiesInfoByAppBuilderIdOut {
    id: number | null;
    customerGuestAppBuilderId: number;
    wifiUsername: string | null;
    wifiPassword: string | null;
    overview: string | null;
    checkInPolicy: string | null;
    termsAndConditions: string | null;
    street: string | null;
    streetNumber: string | null;
    city: string | null;
    postalcode: string | null;
    country: string | null;
    latitude: string | null;
    longitude: string | null;
    roomName: string | null;
    businessType: string | null;
    customerPropertyInfoServicesOuts: CustomerPropertyInfoServicesOut[];
    customerPropertyInfoGalleriesOuts: CustomerPropertyInfoGalleriesOut[];
    customerPropertyInfoEmergencyNumbersOuts: CustomerPropertyInfoEmergencyNumbersOut[];
    customerPropertyInfoExtrasOuts: CustomerPropertyInfoExtrasOut[];
}

export interface CustomerPropertyInfoServicesOut {
    id: number;
    customerPropertyInformationId: number;
    name: string | null;
    icon: string | null;
    description: string | null;
    customerPropertyInfoServiceImagesOuts: CustomerPropertyInfoServiceImagesOut[];
}

export interface CustomerPropertyInfoServiceImagesOut {
    id: number;
    customerPropertyServiceId: number;
    serviceImages: string | null;
}

export interface CustomerPropertyInfoGalleriesOut {
    id: number;
    customerPropertyInformationId: number;
    propertyImage: string | null;
}

export interface CustomerPropertyInfoEmergencyNumbersOut {
    id: number;
    customerPropertyInformationId: number;
    name: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    displayOrder: number | null;
}

export interface CustomerPropertyInfoExtrasOut {
    id: number;
    customerPropertyInformationId: number;
    extraType: number | null;
    name: string | null;
    displayOrder: number | null;
    customerPropertyExtraDetailsOuts: CustomerPropertyInfoExtraDetailsOut[];
}

export interface CustomerPropertyInfoExtraDetailsOut {
    id: number;
    customerPropertyExtraId: number;
    description: string | null;
    latitude: string | null;
    longitude: string | null;
}

}
export namespace CustomersPropertiesInfo.GetCustomersPropertiesInfoById {
export interface GetCustomersPropertiesInfoByIdIn {
    id: number;
}

}
export namespace CustomersPropertiesInfo.GetCustomersPropertiesInfoById {
export interface GetCustomersPropertiesInfoByIdOut extends BaseResponseOut {
    customersPropertiesInfoByIdOut: CustomersPropertiesInfoByIdOut;
}

export interface CustomersPropertiesInfoByIdOut {
    id: number;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    wifiUsername: string | null;
    wifiPassword: string | null;
    overview: string | null;
    checkInPolicy: string | null;
    termsAndConditions: string | null;
    street: string | null;
    streetNumber: string | null;
    city: string | null;
    postalcode: string | null;
    country: string | null;
    latitude: string | null;
    longitude: string | null;
    isActive: boolean | null;
}

}
export namespace CustomersPropertiesInfo.UpdateCustomersPropertiesInfo {
export interface UpdateCustomersPropertiesInfoIn {
    id: number;
    customerGuestAppBuilderId: number | null;
    wifiUsername: string | null;
    wifiPassword: string | null;
    overview: string | null;
    checkInPolicy: string | null;
    termsAndConditions: string | null;
    street: string | null;
    streetNumber: string | null;
    city: string | null;
    postalcode: string | null;
    country: string | null;
    latitude: string | null;
    longitude: string | null;
}

}
export namespace CustomersPropertiesInfo.UpdateCustomersPropertiesInfo {
export interface UpdateCustomersPropertiesInfoOut extends BaseResponseOut {
    updatedCustomersPropertiesInfoOut: UpdatedCustomersPropertiesInfoOut;
}

export interface UpdatedCustomersPropertiesInfoOut {
    id: number;
    customerGuestAppBuilderId: number | null;
    wifiUsername: string | null;
    wifiPassword: string | null;
    overview: string | null;
    checkInPolicy: string | null;
    termsAndConditions: string | null;
    street: string | null;
    streetNumber: string | null;
    city: string | null;
    postalcode: string | null;
    country: string | null;
    latitude: string | null;
    longitude: string | null;
}

}
export namespace CustomerStaffAlerts.CreateCustomerStaffAlerts {
export interface CreateCustomerStaffAlertsIn {
    name: string;
    platfrom: string;
    phoneCountry: string;
    phoneNumber: string;
    waitTimeInMintes: number | null;
    isActive: boolean | null;
    msg: string | null;
    customerUserId: number | null;
}

}
export namespace CustomerStaffAlerts.CreateCustomerStaffAlerts {
export interface CreateCustomerStaffAlertsOut extends BaseResponseOut {
    createdCustomerStaffAlertsOut: CreatedCustomerStaffAlertsOut;
}

export interface CreatedCustomerStaffAlertsOut {
    id: number;
    customerId: number;
    name: string;
    platfrom: string;
    phoneCountry: string;
    phoneNumber: string;
    waitTimeInMintes: number | null;
    isActive: boolean | null;
    msg: string | null;
    customerUserId: number | null;
}

}
export namespace CustomerStaffAlerts.DeleteCustomerStaffAlerts {
export interface DeleteCustomerStaffAlertsIn {
    id: number;
}

}
export namespace CustomerStaffAlerts.DeleteCustomerStaffAlerts {
export interface DeleteCustomerStaffAlertsOut extends BaseResponseOut {
    deletedCustomerStaffAlertsOut: DeletedCustomerStaffAlertsOut;
}

export interface DeletedCustomerStaffAlertsOut {
    id: number;
}

}
export namespace CustomerStaffAlerts.GetCustomerStaffAlertsByCustomerId {
export interface GetCustomerStaffAlertsByCustomerIdIn {

}

}
export namespace CustomerStaffAlerts.GetCustomerStaffAlertsByCustomerId {
export interface GetCustomerStaffAlertsByCustomerIdOut extends BaseResponseOut {
    customerStaffAlertsByCustomerIdOut: CustomerStaffAlertsByCustomerIdOut[];
}

export interface CustomerStaffAlertsByCustomerIdOut {
    id: number;
    customerId: number;
    name: string;
    platfrom: string;
    phoneCountry: string;
    phoneNumber: string;
    waitTimeInMintes: number | null;
    isActive: boolean | null;
    msg: string | null;
    customerUserId: number | null;
}

}
export namespace CustomerStaffAlerts.GetCustomerStaffAlertsById {
export interface GetCustomerStaffAlertsByIdIn {
    id: number;
}

}
export namespace CustomerStaffAlerts.GetCustomerStaffAlertsById {
export interface GetCustomerStaffAlertsByIdOut extends BaseResponseOut {
    customerStaffAlertsByIdOut: CustomerStaffAlertsByIdOut;
}

export interface CustomerStaffAlertsByIdOut {
    id: number;
    customerId: number;
    name: string;
    platfrom: string;
    phoneCountry: string;
    phoneNumber: string;
    waitTimeInMintes: number | null;
    isActive: boolean | null;
}

}
export namespace CustomerStaffAlerts.UpdateCustomerStaffAlerts {
export interface UpdateCustomerStaffAlertsIn {
    id: number;
    name: string;
    platfrom: string;
    phoneCountry: string;
    phoneNumber: string;
    waitTimeInMintes: number | null;
    isActive: boolean | null;
    msg: string | null;
    customerUserId: number | null;
}

}
export namespace CustomerStaffAlerts.UpdateCustomerStaffAlerts {
export interface UpdateCustomerStaffAlertsOut extends BaseResponseOut {
    updatedCustomerStaffAlertsOut: UpdatedCustomerStaffAlertsOut;
}

export interface UpdatedCustomerStaffAlertsOut {
    id: number;
    customerId: number;
    name: string;
    platfrom: string;
    phoneCountry: string;
    phoneNumber: string;
    waitTimeInMintes: number | null;
    isActive: boolean | null;
    msg: string | null;
    customerUserId: number | null;
}

}
export namespace CustomerUsers.CreateCustomerUser {
export interface CreateCustomerUserIn {
    id: number | null;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    title: string | null;
    profilePicture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    departmentId: number | null;
    groupId: number | null;
    customerUserLevelId: number | null;
    supervisorId: number | null;
    userName: string | null;
    password: string | null;
    customerId: number | null;
    isActive: boolean;
    customerUserModulePermissions: CreateCustomerUsersPermissionIn[];
}

export interface CreateCustomerUsersPermissionIn {
    id: number | null;
    permissionId: number | null;
    customerUserId: number | null;
    isView: boolean | null;
    isEdit: boolean | null;
    isUpload: boolean | null;
    isReply: boolean | null;
    isDownload: boolean | null;
}

}
export namespace CustomerUsers.CreateCustomerUser {
export interface CreateCustomerUserOut extends BaseResponseOut {

}

}
export namespace CustomerUsers.EditCustomerUser {
export interface EditCustomerUserIn {
    id: number | null;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    title: string | null;
    profilePicture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    departmentId: number | null;
    groupId: number | null;
    customerUserLevelId: number | null;
    supervisorId: number | null;
    userName: string | null;
    password: string | null;
    customerId: number | null;
    isActive: boolean;
    customerUserModulePermissions: EditCustomerUsersPermissionIn[];
}

export interface EditCustomerUsersPermissionIn {
    id: number | null;
    permissionId: number | null;
    customerUserId: number | null;
    isView: boolean | null;
    isEdit: boolean | null;
    isUpload: boolean | null;
    isReply: boolean | null;
    isDownload: boolean | null;
}

}
export namespace CustomerUsers.EditCustomerUser {
export interface EditCustomerUserOut extends BaseResponseOut {

}

}
export namespace CustomerUsers.GetCustomerInfoByWidgetChatId {
export interface GetCustomerInfoByWidgetChatIdIn {
    widgetChatId: string | null;
}

}
export namespace CustomerUsers.GetCustomerInfoByWidgetChatId {
export interface GetCustomerInfoByWidgetChatIdOut extends BaseResponseOut {
    getCustomerInfoByWidgetChatIdResponseOut: GetCustomerInfoByWidgetChatIdResponseOut | null;
}

export interface GetCustomerInfoByWidgetChatIdResponseOut {
    logo: string | null;
    color: string | null;
    customerUserId: number | null;
    chatWidgetPortal: string | null;
    cname: string | null;
}

}
export namespace CustomerUsers.GetCustomerSupervisorUsers {
export interface GetCustomerSupervisorUsersIn {
    departmentId: number | null;
    groupId: number | null;
    customerUserLevelId: number | null;
    customerUserId: number | null;
    customerId: number | null;
}

}
export namespace CustomerUsers.GetCustomerSupervisorUsers {
export interface GetCustomerSupervisorUsersOut extends BaseResponseOut {
    customerUserOut: CustomerUserOut[];
}

export interface CustomerUserOut {
    id: number | null;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    title: string | null;
    profilePicture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    departmentId: number | null;
    customerUserLevelId: number | null;
    isActive: boolean;
    customerUserModulePermissions: CustomerUsersPermissionOut[];
}

export interface CustomerUsersPermissionOut {
    id: number;
    customerPermissionId: number | null;
    customerUserId: number | null;
    isView: boolean | null;
    isEdit: boolean | null;
    isUpload: boolean | null;
    isReply: boolean | null;
    isDownload: boolean | null;
}

}
export namespace CustomerUsers.GetCustomerUserByCustomerId {
export interface GetCustomerUserByCustomerIdIn {

}

}
export namespace CustomerUsers.GetCustomerUserByCustomerId {
export interface GetCustomerUserByCustomerIdOut extends BaseResponseOut {
    customerStaffsByCustomerIdOut: CustomerUsersByCustomerIdOut[];
}

export interface CustomerUsersByCustomerIdOut {
    id: number | null;
    name: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
}

}
export namespace CustomerUsers.GetCustomerUserById {
export interface GetCustomerUserByIdIn {
    id: number;
}

}
export namespace CustomerUsers.GetCustomerUserById {
export interface GetCustomerUserByIdOut extends BaseResponseOut {
    customerUserByIdOut: CustomerUserByIdOut;
}

export interface CustomerUserByIdOut {
    id: number | null;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    title: string | null;
    profilePicture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    departmentId: number | null;
    groupId: number | null;
    customerUserLevelId: number | null;
    supervisorId: number | null;
    userName: string | null;
    password: string | null;
    isActive: boolean;
    customerUserModulePermissions: CustomerUserPermissionsOut[];
}

export interface CustomerUserPermissionsOut {
    id: number | null;
    permissionId: number | null;
    customerUserId: number | null;
    isView: boolean | null;
    isEdit: boolean | null;
    isUpload: boolean | null;
    isReply: boolean | null;
    isDownload: boolean | null;
}

}
export namespace CustomerUsers.GetDepartmentCustomerUsers {
export interface GetDepartmentCustomerUsersIn extends BaseSearchFilterOptions {
    customerId: number | null;
}

}
export namespace CustomerUsers.GetDepartmentCustomerUsers {
export interface GetDepartmentCustomerUsersOut extends BaseResponseOut {
    customerDeptWiseUserOut: CustomerDepartmentsOut[];
}

export interface CustomerDepartmentsOut {
    id: number | null;
    name: string | null;
    ceoId: number | null;
    ceoName: string | null;
    managerId: number | null;
    managerName: string | null;
    filteredCount: number | null;
    isActive: boolean;
    customerGroups: CustomerGroupOut[] | null;
}

export interface CustomerGroupOut {
    id: number | null;
    name: string | null;
    groupLeaderId: number | null;
    groupLeader: string | null;
    isActive: boolean;
    customerUsersOut: CustomerUserOut[] | null;
}

export interface CustomerUserOut {
    id: number | null;
    firstName: string | null;
    lastName: string | null;
    isActive: boolean;
}

}
export namespace CustomerUsers.UpdateCustomerUserStatus {
export interface UpdateCustomerUserStatusIn {
    customerUserId: number;
    isActive: boolean;
}

}
export namespace CustomerUsers.UpdateCustomerUserStatus {
export interface UpdateCustomerUserStatusOut extends BaseResponseOut {

}

}
export namespace CustomerUsers.UpdateMuteStatus {
export interface UpdateMuteStatusIn {
    customerUserId: number | null;
    isMuted: boolean;
}

}
export namespace CustomerUsers.UpdateMuteStatus {
export interface UpdateMuteStatusOut extends BaseResponseOut {

}

}
export namespace Department.CreateCustomerDepartment {
export interface CreateCustomerDepartmentIn {
    name: string;
    isActive: boolean;
    customerId: number | null;
}

}
export namespace Department.CreateCustomerDepartment {
export interface CreateCustomerDepartmentOut extends BaseResponseOut {

}

}
export namespace Department.CreateDepartment {
export interface CreateDepartmentIn {
    name: string;
    isActive: boolean;
}

}
export namespace Department.CreateDepartment {
export interface CreateDepartmentOut extends BaseResponseOut {

}

}
export namespace Department.EditDepartment {
export interface EditDepartmentIn {
    name: string;
    isActive: boolean | null;
    userType: number | null;
}

}
export namespace Department.EditDepartment {
export interface EditDepartmentOut extends BaseResponseOut {

}

}
export namespace Department.GetCustomerDepartmentById.cs {
export interface GetCustomerDepartmentByIdIn {

}

}
export namespace Department.GetCustomerDepartmentById.cs {
export interface GetCustomerDepartmentByIdOut extends BaseResponseOut {
    getCustomerDepartmentByIdResponseOut: GetCustomerDepartmentByIdResponseOut;
}

export interface GetCustomerDepartmentByIdResponseOut {
    id: number;
    name: string | null;
    departmentMangerId: number | null;
}

}
export namespace Department.GetDepartmentById {
export interface GetDepartmentByIdIn {

}

}
export namespace Department.GetDepartmentById {
export interface GetDepartmentByIdOut extends BaseResponseOut {
    getDepartmentByIdResponseOut: GetDepartmentByIdResponseOut;
}

export interface GetDepartmentByIdResponseOut {
    id: number;
    name: string | null;
    departmentMangerId: number | null;
}

}
export namespace Department.GetDepartments {
export interface GetDepartmentsIn {
    userType: number | null;
    userId: number | null;
}

}
export namespace Department.GetDepartments {
export interface GetDepartmentsOut extends BaseResponseOut {
    getDepartmentsResponseOut: GetDepartmentsResponseOut[];
}

export interface GetDepartmentsResponseOut {
    id: number;
    name: string | null;
    departmentMangerId: number | null;
}

}
export namespace Displayorder.GetDisplayOrder {
export interface GetDisplayOrderIn {
    referenceId: number;
}

}
export namespace Displayorder.GetDisplayOrder {
export interface GetDisplayOrderOut extends BaseResponseOut {
    displayOrderOut: DisplayOrderOut;
}

export interface DisplayOrderOut {
    id: number;
    screenName: number | null;
    jsonData: string | null;
    refrenceId: number;
}

}
export namespace Displayorder.UpdateDisplayorder {
export interface UpdateDisplayorderIn {
    id: number;
    screenName: number | null;
    jsonData: string | null;
    refrenceId: number;
}

}
export namespace Displayorder.UpdateDisplayorder {
export interface UpdateDisplayorderOut extends BaseResponseOut {

}

}
export namespace FeaturePermission.GetCustomerFeaturePermissions {
export interface GetCustomerFeaturePermissionsIn {

}

}
export namespace FeaturePermission.GetCustomerFeaturePermissions {
export interface GetCustomerFeaturePermissionsOut extends BaseResponseOut {
    getCustomerFeaturePermissionsResponseOut: GetCustomerFeaturePermissionsResponseOut[];
}

export interface GetCustomerFeaturePermissionsResponseOut {
    id: number;
    name: string;
    value: string;
    isView: boolean | null;
    isEdit: boolean | null;
    isUpload: boolean | null;
    isReply: boolean | null;
    isDownload: boolean | null;
}

}
export namespace FeaturePermission.GetFeaturePermissions {
export interface GetFeaturePermissionsIn {

}

}
export namespace FeaturePermission.GetFeaturePermissions {
export interface GetFeaturePermissionsOut extends BaseResponseOut {
    getFeaturePermissionsResponseOut: GetFeaturePermissionsResponseOut[];
}

export interface GetFeaturePermissionsResponseOut {
    id: number;
    name: string;
    isView: boolean | null;
    isEdit: boolean | null;
    isReply: boolean | null;
    isUpload: boolean | null;
    isSend: boolean | null;
}

}
export namespace Files.CreateFile {
export interface CreateFileIn {
    file: any;
    documentType: string;
    containerName: string | null;
}

}
export namespace Files.CreateFile {
export interface CreateFileOut extends BaseResponseOut {
    memoryStream: any;
    fileName: string;
    location: string;
    contentType: string;
    tempSasUri: string;
}

export interface CreateFileOutV1 extends BaseResponseOut {
    fileName: string;
    location: string;
    contentType: string;
    tempSasUri: string;
    expireAt: string;
}

}
export namespace Files.GetCommunicationFile {
export interface GetCommunicationFileIn {
    location: string;
}

}
export namespace Files.GetCommunicationFile {
export interface GetCommunicationFileOut extends BaseResponseOut {
    memoryStream: any;
    fileDownloadName: string;
    contentType: string;
}

export interface GetCommunicationFileOutV1 extends BaseResponseOut {
    fileDownloadName: string;
    contentType: string;
    tempSasUri: string;
    expireAt: string;
}

}
export namespace Files.GetFile {
export interface GetFileIn {
    location: string;
}

}
export namespace Files.GetFile {
export interface GetFileOut extends BaseResponseOut {
    memoryStream: any;
    fileDownloadName: string;
    contentType: string;
}

export interface GetFileOutV1 extends BaseResponseOut {
    fileDownloadName: string;
    contentType: string;
    tempSasUri: string;
    expireAt: string;
}

}
export namespace Files.UploadCommunicationFile {
export interface UploadCommunicationFileIn {
    file: any;
    documentType: string;
}

}
export namespace Files.UploadCommunicationFile {
export interface UploadCommunicationFileOut extends BaseResponseOut {
    memoryStream: any;
    fileName: string;
    location: string;
    contentType: string;
    tempSasUri: string;
}

export interface UploadCommunicationFileOutV1 extends BaseResponseOut {
    fileName: string;
    location: string;
    contentType: string;
    tempSasUri: string;
    expireAt: string;
}

}
export namespace Gr4vyPaymentService.CreateGr4vyPaymentService {
export interface CreateGr4vyPaymentServiceIn {
    paymentProcessorId: number | null;
    isDigitalWallet: boolean;
    isActive: boolean;
    paymentService: PaymentService | null;
    digitalWallet: DigitalWallet | null;
}

export interface PaymentService {
    payment_service_definition_id: string | null;
    display_name: string | null;
    fields: Field[] | null;
    accepted_currencies: string[] | null;
    accepted_countries: string[] | null;
}

export interface DigitalWallet {
    accept_terms_and_conditions: boolean | null;
    domain_names: string[] | null;
    merchant_name: string | null;
    merchant_url: string | null;
    provider: string | null;
}

export interface Field {
    key: string | null;
    value: string | null;
}

}
export namespace Gr4vyPaymentService.CreateGr4vyPaymentService {
export interface CreateGr4vyPaymentServiceOut extends BaseResponseOut {
    paymentServiceOut: PaymentServiceOut;
}

export interface PaymentServiceOut {
    gRWebhookURL: string | null;
}

}
export namespace Gr4vyPaymentService.GetGr4vyPaymentService {
export interface GetGr4vyPaymentServiceIn {

}

}
export namespace Gr4vyPaymentService.GetGr4vyPaymentService {
export interface GetGr4vyPaymentServiceOut extends BaseResponseOut {
    hospitioPaymentServices: HospitioPaymentServicesOut[];
}

export interface HospitioPaymentServicesOut {
    hospitioPaymentProcessorId: number | null;
    isActive: boolean | null;
    gRCategory: string | null;
    gRGroup: string | null;
    gRIcon: string | null;
    gRName: string | null;
}

}
export namespace Gr4vyPaymentService.GetGr4vyPaymentServiceById {
export interface GetGr4vyPaymentServiceByIdIn {
    hospitioPaymentProcessorId: number;
}

}
export namespace Gr4vyPaymentService.GetGr4vyPaymentServiceById {
export interface GetGr4vyPaymentServiceByIdOut extends BaseResponseOut {
    paymentService: PaymentServiceByIdOut;
}

export interface PaymentServiceByIdOut {
    id: number | null;
    paymentProcessorId: number | null;
    gRPaymentServiceId: string | null;
    gRWebhookURL: string | null;
    isActive: boolean | null;
    gR3DSecureEnabled: boolean | null;
    gRAcceptedCountries: string | null;
    gRAcceptedCurrencies: string | null;
    gRFields: string | null;
    gRIsDeleted: boolean | null;
    gRMerchantProfile: string | null;
    paymentProcessorsOuts: PaymentProcessorsOuts;
    paymentProcessorsDefinationsOuts: PaymentProcessorsDefinationsOuts;
}

export interface PaymentProcessorsOuts {
    id: number | null;
    isActive: boolean | null;
    gRCategory: string | null;
    gRGroup: string | null;
    gRID: string | null;
    gRIcon: string | null;
    gRName: string | null;
}

export interface PaymentProcessorsDefinationsOuts {
    id: number | null;
    gRFields: string | null;
    gRSupportedCountries: string | null;
    gRSupportedCurrencies: string | null;
    gRSupportedFeatures: string | null;
    paymentProcessorId: number | null;
    isActive: boolean | null;
}

}
export namespace Gr4vyPaymentService.UpdateGr4vyPaymentService {
export interface UpdateGr4vyPaymentServiceIn {
    hospitioPaymentProcessorId: number | null;
    isDigitalWallet: boolean;
    isActive: boolean;
    paymentService: PaymentService | null;
    digitalWallet: DigitalWallet | null;
}

export interface PaymentService {
    payment_service_definition_id: string | null;
    accepted_currencies: string[] | null;
    accepted_countries: string[] | null;
    display_name: string | null;
    fields: Field[] | null;
}

export interface DigitalWallet {
    accept_terms_and_conditions: boolean | null;
    domain_names: string[] | null;
    merchant_name: string | null;
    merchant_url: string | null;
    provider: string | null;
}

}
export namespace Gr4vyPaymentService.UpdateGr4vyPaymentService {
export interface UpdateGr4vyPaymentServiceOut extends BaseResponseOut {
    paymentServiceOut: PaymentServiceOut;
}

export interface PaymentServiceOut {
    gRWebhookURL: string | null;
}

}
export namespace Gr4vyPaymentService.VerifyGr4vyPaymentService {
export interface VerifyGr4vyPaymentServiceIn {
    paymentProcessorId: number;
    fields: Field[];
}

export interface Field {
    key: string | null;
    value: string | null;
}

}
export namespace Gr4vyPaymentService.VerifyGr4vyPaymentService {
export interface VerifyGr4vyPaymentServiceOut extends BaseResponseOut {
    verify: VerifyPaymentServiceOut;
}

export interface VerifyPaymentServiceOut {
    isVerifySuccess: boolean;
}

}
export namespace Gr4vyPaymentServiceCustomer.CreateCustomerGr4vyPaymentService {
export interface CreateCustomerGr4vyPaymentServiceIn {
    customerId: number | null;
    paymentProcessorId: number | null;
    isDigitalWallet: boolean;
    isActive: boolean;
    paymentService: PaymentService | null;
    digitalWallet: DigitalWallet | null;
}

export interface PaymentService {
    payment_service_definition_id: string | null;
    display_name: string | null;
    fields: Field[] | null;
    accepted_currencies: string[] | null;
    accepted_countries: string[] | null;
}

export interface DigitalWallet {
    accept_terms_and_conditions: boolean | null;
    domain_names: string[] | null;
    merchant_name: string | null;
    merchant_url: string | null;
    provider: string | null;
}

export interface Field {
    key: string | null;
    value: string | null;
}

}
export namespace Gr4vyPaymentServiceCustomer.CreateCustomerGr4vyPaymentService {
export interface CreateCustomerGr4vyPaymentServiceOut extends BaseResponseOut {
    paymentServiceOut: PaymentServiceOut;
}

export interface PaymentServiceOut {
    gRWebhookURL: string | null;
}

}
export namespace Gr4vyPaymentServiceCustomer.GetCustomerGr4vyPaymentService {
export interface GetCustomerGr4vyPaymentServiceIn {
    customerId: number | null;
}

}
export namespace Gr4vyPaymentServiceCustomer.GetCustomerGr4vyPaymentService {
export interface GetCustomerGr4vyPaymentServiceOut extends BaseResponseOut {
    customerPaymentServices: CustomerPaymentServicesOut[];
}

export interface CustomerPaymentServicesOut {
    customerPaymentProcessorId: number | null;
    isActive: boolean | null;
    gRCategory: string | null;
    gRGroup: string | null;
    gRIcon: string | null;
    gRName: string | null;
}

}
export namespace Gr4vyPaymentServiceCustomer.GetCustomerGr4vyPaymentServiceById {
export interface GetCustomerGr4vyPaymentServiceByIdIn {
    customerPaymentProcessorId: number;
}

}
export namespace Gr4vyPaymentServiceCustomer.GetCustomerGr4vyPaymentServiceById {
export interface GetCustomerGr4vyPaymentServiceByIdOut extends BaseResponseOut {
    paymentService: CustomerPaymentServiceByIdOut;
}

export interface CustomerPaymentServiceByIdOut {
    id: number | null;
    paymentProcessorId: number | null;
    gRPaymentServiceId: string | null;
    gRWebhookURL: string | null;
    isActive: boolean | null;
    gR3DSecureEnabled: boolean | null;
    gRAcceptedCountries: string | null;
    gRAcceptedCurrencies: string | null;
    gRFields: string | null;
    gRIsDeleted: boolean | null;
    gRMerchantProfile: string | null;
    paymentProcessorsOuts: CustomerPaymentProcessorsOuts;
    paymentProcessorsDefinationsOuts: CustomerPaymentProcessorsDefinationsOuts;
}

export interface CustomerPaymentProcessorsOuts {
    id: number | null;
    isActive: boolean | null;
    gRCategory: string | null;
    gRGroup: string | null;
    gRID: string | null;
    gRIcon: string | null;
    gRName: string | null;
}

export interface CustomerPaymentProcessorsDefinationsOuts {
    id: number | null;
    gRFields: string | null;
    gRSupportedCountries: string | null;
    gRSupportedCurrencies: string | null;
    gRSupportedFeatures: string | null;
    paymentProcessorId: number | null;
    isActive: boolean | null;
}

}
export namespace Gr4vyPaymentServiceCustomer.UpdateCustomerGr4vyPaymentService {
export interface UpdateCustomerGr4vyPaymentServiceIn {
    customerId: number | null;
    customerPaymentProcessorId: number | null;
    isDigitalWallet: boolean;
    isActive: boolean;
    paymentService: PaymentService | null;
    digitalWallet: DigitalWallet | null;
}

export interface PaymentService {
    payment_service_definition_id: string | null;
    accepted_currencies: string[] | null;
    accepted_countries: string[] | null;
    display_name: string | null;
    fields: Field[] | null;
}

export interface DigitalWallet {
    accept_terms_and_conditions: boolean | null;
    domain_names: string[] | null;
    merchant_name: string | null;
    merchant_url: string | null;
    provider: string | null;
}

export interface Field {
    key: string | null;
    value: string | null;
}

}
export namespace Gr4vyPaymentServiceCustomer.UpdateCustomerGr4vyPaymentService {
export interface UpdateCustomerGr4vyPaymentServiceOut extends BaseResponseOut {
    paymentServiceOut: PaymentServiceOut;
}

export interface PaymentServiceOut {
    gRWebhookURL: string | null;
}

}
export namespace Gr4vyPaymentServiceCustomer.VerifyCustomerGr4vyPaymentService {
export interface VerifyCustomerGr4vyPaymentServiceIn {
    customerId: number | null;
    paymentProcessorId: number;
    fields: Field[];
}

export interface Field {
    key: string | null;
    value: string | null;
}

}
export namespace Gr4vyPaymentServiceCustomer.VerifyCustomerGr4vyPaymentService {
export interface VerifyCustomerGr4vyPaymentServiceOut extends BaseResponseOut {
    verifyCustomer: VerifyCustomerPaymentServiceOut;
}

export interface VerifyCustomerPaymentServiceOut {
    isVerifySuccess: boolean;
}

}
export namespace Groups.CreateGroup {
export interface CreateGroupIn {
    id: number;
    name: string;
    departmentId: number;
    isActive: boolean;
    userType: number | null;
    userId: number | null;
}

}
export namespace Groups.CreateGroup {
export interface CreateGroupOut extends BaseResponseOut {
    createdGroupOut: CreatedGroupOut;
}

export interface CreatedGroupOut {
    id: number;
    name: string;
    departmentId: number;
    isActive: boolean;
}

}
export namespace Groups.DeleteGroup {
export interface DeleteGroupIn {
    groupId: number;
    userType: number | null;
}

}
export namespace Groups.DeleteGroup {
export interface DeleteGroupOut extends BaseResponseOut {
    deleteGroup: DeleteGroup;
}

export interface DeleteGroup {
    deleteGroupId: number;
}

}
export namespace Groups.GetGroup {
export interface GetGroupIn {
    id: number;
    userType: number | null;
}

}
export namespace Groups.GetGroup {
export interface GetGroupOut extends BaseResponseOut {
    groupOut: GroupOut;
}

export interface GroupOut {
    id: number;
    name: string | null;
    departmentId: number | null;
    isActive: boolean | null;
}

}
export namespace Groups.GetGroups {
export interface GetGroupsIn {
    userType: number | null;
    userId: number | null;
}

}
export namespace Groups.GetGroups {
export interface GetGroupsOut extends BaseResponseOut {
    groupsOut: GroupsOut[];
}

export interface GroupsOut {
    id: number;
    name: string | null;
    departmentId: number | null;
    isActive: boolean | null;
}

}
export namespace Groups.GetGroupsByDepartmentId {
export interface GetGroupsByDepartmentIdIn {
    departmentId: number;
    userType: number | null;
}

}
export namespace Groups.GetGroupsByDepartmentId {
export interface GetGroupsByDepartmentIdOut extends BaseResponseOut {
    groupsOut: GroupsByDepartmentIdOut[];
}

export interface GroupsByDepartmentIdOut {
    id: number;
    name: string | null;
}

}
export namespace Groups.UpdateGroup {
export interface UpdateGroupIn {
    id: number;
    name: string | null;
    departmentId: number;
    isActive: boolean;
    userType: number | null;
}

}
export namespace Groups.UpdateGroup {
export interface UpdateGroupOut extends BaseResponseOut {
    updatedGroupOut: UpdatedGroupOut;
}

export interface UpdatedGroupOut {
    groupId: number;
    groupName: string;
    departmentId: number;
    isActive: boolean;
}

}
export namespace GuestAppEnhanceYourStayItemImage.CreateGuestAppEnhanceYourStayItemsImages {
export interface CreateGuestAppEnhanceYourStayItemImageIn {
    id: number;
    customerGuestAppEnhanceYourStayItemId: number | null;
    itemsImages: GuestAppEnhanceYourStayItemAttachementIn[];
}

export interface GuestAppEnhanceYourStayItemAttachementIn {
    itemsImage: string | null;
    displayOrder: number | null;
}

}
export namespace GuestAppEnhanceYourStayItemImage.CreateGuestAppEnhanceYourStayItemsImages {
export interface CreateGuestAppEnhanceYourStayItemImageOut extends BaseResponseOut {
    createdEnhanceYourStayItemOut: CreatedEnhanceYourStayItemImageOut[];
}

export interface CreatedEnhanceYourStayItemImageOut {
    id: number;
    customerGuestAppEnhanceYourStayItemId: number | null;
    itemsImages: string | null;
    disaplayOrder: number | null;
}

}
export namespace GuestAppEnhanceYourStayItemImage.DeleteGuestAppEnhanceYourStayItemsImages {
export interface DeleteGuestAppEnhanceYourStayItemsImagesIn {
    customerGuestAppEnhanceYourStayItemId: number;
}

}
export namespace GuestAppEnhanceYourStayItemImage.DeleteGuestAppEnhanceYourStayItemsImages {
export interface DeleteGuestAppEnhanceYourStayItemsImagesOut extends BaseResponseOut {
    removeQuestionAnswerOut: RemoveEnhanceYourStayItemImageOut;
}

export interface RemoveEnhanceYourStayItemImageOut {
    customerGuestAppEnhanceYourStayItemId: number;
}

}
export namespace GuestAppEnhanceYourStayItemImage.GetGuestAppEnhanceYourStayItemImages {
export interface GetGuestAppEnhanceYourStayItemImagesIn {
    searchColumn: string | null;
    searchValue: string | null;
    pageNo: number;
    pageSize: number;
    sortColumn: string | null;
    sortOrder: string | null;
    customerGuestAppEnhanceYourStayItemId: number;
}

}
export namespace GuestAppEnhanceYourStayItemImage.GetGuestAppEnhanceYourStayItemImages {
export interface GetGuestAppEnhanceYourStayItemImagesOut extends BaseResponseOut {
    guestAppEnhanceYourStayItemImageOuts: GuestAppEnhanceYourStayItemImageOut[];
}

export interface GuestAppEnhanceYourStayItemImageOut {
    id: number;
    customerGuestAppEnhanceYourStayItemId: number | null;
    itemsImages: string | null;
    disaplayOrder: number | null;
    isActive: boolean | null;
    filteredCount: number | null;
}

}
export namespace GuestJourneyMessagesTemplates.CreateGuestJourneyMessagesTemplates {
export interface CreateGuestJourneyMessagesTemplatesIn {
    userId: number;
    name: string;
    templeteType: number | null;
    templetMessage: string | null;
    isActive: boolean | null;
    buttons: Button[] | null;
}

export interface Button {
    type: string | null;
    text: string | null;
    value: string | null;
}

}
export namespace GuestJourneyMessagesTemplates.CreateGuestJourneyMessagesTemplates {
export interface CreateGuestJourneyMessagesTemplatesOut extends BaseResponseOut {
    createdGuestJourneyMessagesTemplatesOut: CreatedGuestJourneyMessagesTemplatesOut;
}

export interface CreatedGuestJourneyMessagesTemplatesOut {
    id: number;
    name: string;
    whatsappTemplateName: string;
    templeteType: number | null;
    templetMessage: string | null;
    vonageTemplateId: string | null;
    vonageTemplateStatus: string | null;
    buttons: string | null;
}

}
export namespace GuestJourneyMessagesTemplates.DeleteGuestJourneyMessagesTemplates {
export interface DeleteGuestJourneyMessagesTemplatesIn {
    id: number;
}

}
export namespace GuestJourneyMessagesTemplates.DeleteGuestJourneyMessagesTemplates {
export interface DeleteGuestJourneyMessagesTemplatesOut extends BaseResponseOut {
    deletedGuestJourneyMessagesTemplatesOut: DeletedGuestJourneyMessagesTemplatesOut;
}

export interface DeletedGuestJourneyMessagesTemplatesOut {
    id: number;
}

}
export namespace GuestJourneyMessagesTemplates.GetGuestJourneyMessagesTemplateById {
export interface GetGuestJourneyMessagesTemplateByIdIn {
    id: number;
}

}
export namespace GuestJourneyMessagesTemplates.GetGuestJourneyMessagesTemplateById {
export interface GetGuestJourneyMessagesTemplateByIdOut extends BaseResponseOut {
    guestJourneyMessagesTemplateByIdOut: GuestJourneyMessagesTemplateByIdOut;
}

export interface GuestJourneyMessagesTemplateByIdOut {
    id: number;
    name: string;
    templeteType: number | null;
    templetMessage: string | null;
    buttons: string | null;
    vonageTemplateId: string | null;
    vonageTemplateStatus: string | null;
    isActive: boolean | null;
}

}
export namespace GuestJourneyMessagesTemplates.GetGuestJourneyMessagesTemplates {
export interface GetGuestJourneyMessagesTemplatesIn {

}

}
export namespace GuestJourneyMessagesTemplates.GetGuestJourneyMessagesTemplates {
export interface GetGuestJourneyMessagesTemplatesOut extends BaseResponseOut {
    guestJourneyMessagesTemplatesOut: GuestJourneyMessagesTemplatesOut[];
}

export interface GuestJourneyMessagesTemplatesOut {
    id: number;
    name: string;
    templeteType: number | null;
    templetMessage: string | null;
    buttons: string | null;
    vonageTemplateId: string | null;
    vonageTemplateStatus: string | null;
    isActive: boolean | null;
}

}
export namespace GuestJourneyMessagesTemplates.GetGuestJourneyMessagesTemplatesForCustomer {
export interface GetGuestJourneyMessagesTemplatesForCustomerIn {

}

}
export namespace GuestJourneyMessagesTemplates.GetGuestJourneyMessagesTemplatesForCustomer {
export interface GetGuestJourneyMessagesTemplatesForCustomerOut extends BaseResponseOut {
    guestJourneyMessagesTemplatesOut: GuestJourneyMessagesTemplatesForCustomerOut[];
}

export interface GuestJourneyMessagesTemplatesForCustomerOut {
    id: number;
    name: string;
    templeteType: number | null;
    templetMessage: string | null;
    buttons: string | null;
    vonageTemplateId: string | null;
    vonageTemplateStatus: string | null;
    isActive: boolean | null;
}

}
export namespace GuestJourneyMessagesTemplates.UpdateGuestJourneyMessagesTemplates {
export interface UpdateGuestJourneyMessagesTemplatesIn {
    id: number;
    userId: number;
    name: string;
    templeteType: number | null;
    templetMessage: string | null;
    isActive: boolean | null;
    buttons: Button[] | null;
}

export interface Button {
    type: string | null;
    text: string | null;
    value: string | null;
}

}
export namespace GuestJourneyMessagesTemplates.UpdateGuestJourneyMessagesTemplates {
export interface UpdateGuestJourneyMessagesTemplatesOut extends BaseResponseOut {
    updatedGuestJourneyMessagesTemplatesOut: UpdatedGuestJourneyMessagesTemplatesOut;
}

export interface UpdatedGuestJourneyMessagesTemplatesOut {
    id: number;
    name: string;
    whatsappTemplateName: string;
    templeteType: number | null;
    templetMessage: string | null;
    vonageTemplateId: string | null;
    vonageTemplateStatus: string | null;
    isActive: boolean | null;
    buttons: string | null;
}

}
export namespace GuestRequest.CreateGuestRequest {
export interface CreateGuestRequestIn {
    guestRequests: GuestRequestIn[];
}

export interface GuestRequestIn {
    customerId: number;
    requestType: GuestRequestTypeEnum;
    customerGuestAppConciergeItemId: number | null;
    customerGuestAppHousekeepingItemId: number | null;
    customerGuestAppRoomServiceItemId: number | null;
    customerGuestAppReceptionItemId: number | null;
    guestId: number;
    monthValue: number | null;
    dayValue: number | null;
    yearValue: number | null;
    hourValue: number | null;
    minuteValue: number | null;
    pickupLocation: string | null;
    destination: string | null;
    comment: string | null;
    paymentId: string | null;
    paymentDetails: string | null;
    status: GuestRequestStatusEnum | null;
    isActive: boolean;
    quantityBar: number | null;
}

}
export namespace GuestRequest.CreateGuestRequest {
export interface CreateGuestRequestOut extends BaseResponseOut {

}

}
export namespace GuestRequest.GetGuestEnhanceStayRequests {
export interface GetGuestEnhanceStayRequestsIn {
    customerId: number | null;
    guestId: number | null;
}

}
export namespace GuestRequest.GetGuestEnhanceStayRequests {
export interface GetGuestEnhanceStayRequestsOut extends BaseResponseOut {
    guestRequestsOut: GuestRequestsOut[];
}

export interface GuestRequestsOut {
    name: string | null;
    createdAt: string | null;
    status: number | null;
    requestType: number | null;
}

}
export namespace GuestRequest.GetGuestRequestById {
export interface GetGuestRequestByIdIn {
    id: number;
}

}
export namespace GuestRequest.GetGuestRequestById {
export interface GetGuestRequestByIdOut extends BaseResponseOut {
    guestRequestByIdOut: GuestRequestByIdOut;
}

export interface GuestRequestByIdOut {
    id: number;
    firstName: string | null;
    lastName: string | null;
    taskStatus: number | null;
    room: string | null;
    requestType: number | null;
    enhanceYourStayItem: string | null;
    enhanceYourStayItemPrice: number | null;
    houseKeepingItem: string | null;
    houseKeepingItemPrice: number | null;
    conciergeItem: string | null;
    conciergeItemPrice: number | null;
    receptionItem: string | null;
    receptionItemPrice: number | null;
    roomServiceItem: string | null;
    roomServiceItemPrice: number | null;
    rating: number;
    monthValue: number | null;
    dayValue: number | null;
    yearValue: number | null;
    hourValue: number | null;
    minuteValue: number | null;
    timeStamp: string | null;
    updateAt: string | null;
}

}
export namespace GuestRequest.GetGuestRequests {
export interface GetGuestRequestsIn {
    pageNo: number | null;
    pageSize: number | null;
    sortColumn: string | null;
    sortOrder: string | null;
}

}
export namespace GuestRequest.GetGuestRequests {
export interface GetGuestRequestsOut extends BaseResponseOut {
    guestRequestsOut: GuestRequestsOut[];
}

export interface GuestRequestsOut {
    id: number;
    guestId: number;
    guestName: string | null;
    guestStatus: string | null;
    department: string | null;
    taskItem: string | null;
    charge: number | null;
    timeStamp: string | null;
    taskStatus: number | null;
    rating: number | null;
    totalCount: number | null;
    guestRequest: string | null;
}

}
export namespace GuestRequest.UpdateGuestRequest {
export interface UpdateGuestRequestIn {
    id: number;
    status: GuestRequestStatusEnum;
    isActive: boolean;
}

}
export namespace GuestRequest.UpdateGuestRequest {
export interface UpdateGuestRequestOut extends BaseResponseOut {

}

}
export namespace GuestRequest.UpdateGuestRequestStatus {
export interface UpdateGuestRequestStatusIn {
    guestRequestId: number | null;
    enhanceStayGuestRequestId: number | null;
    status: GuestRequestStatusEnum;
}

}
export namespace GuestRequest.UpdateGuestRequestStatus {
export interface UpdateGuestRequestStatusOut extends BaseResponseOut {

}

}
export namespace GuestRequestEnhanceStayItem.CreateGuestRequestEnhanceStayItem {
export interface CreateGuestRequestEnhanceStayItemIn {
    customerId: number;
    guestId: number;
    customerGuestAppEnhanceYourStayItemId: number;
    qty: number | null;
    paymentId: string | null;
    paymentDetails: string | null;
    status: number | null;
    enhanceStayItemExtraIns: EnhanceStayItemExtraIn[];
}

export interface EnhanceStayItemExtraIn {
    customerGuestAppEnhanceYourStayCategoryItemsExtraId: number;
    month: number | null;
    day: number | null;
    year: number | null;
    hour: number | null;
    minute: number | null;
    pickupLocation: string | null;
    qunatity: number | null;
    destination: string | null;
    comment: string | null;
    status: number | null;
}

}
export namespace GuestRequestEnhanceStayItem.CreateGuestRequestEnhanceStayItem {
export interface CreateGuestRequestEnhanceStayItemOut extends BaseResponseOut {

}

}
export namespace HospitioAdminCommunication.GetAdminUserCustomers {
export interface GetAdminUserCustomersIn {
    searchString: string;
    pageNo: number | null;
    pageSize: number | null;
    userId: number | null;
}

}
export namespace HospitioAdminCommunication.GetAdminUserCustomers {
export interface GetAdminUserCustomersOut extends BaseResponseOut {
    adminUserCustomersOut: AdminUserCustomersOut[];
}

export interface AdminUserCustomersOut {
    id: number | null;
    businessName: string | null;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    title: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    userType: string | null;
    userName: string | null;
    profilePicture: string | null;
    userOuts: UserOuts[];
}

export interface UserOuts {
    id: number | null;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    profilePicture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
}

}
export namespace HospitioAdminCommunication.GetAdminUserCustomersDetail {
export interface GetAdminUserCustomersDetailIn {
    customerId: number;
    userType: string;
}

}
export namespace HospitioAdminCommunication.GetAdminUserCustomersDetail {
export interface GetAdminUserCustomersDetailOut extends BaseResponseOut {
    getAdminUserCustomersDetailResponseOut: GetAdminUserCustomersDetailResponseOut;
}

export interface GetAdminUserCustomersDetailResponseOut {
    userId: number | null;
    businessName: string | null;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    profilePicture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    incomingTranslationLangage: string | null;
    noOfRooms: number | null;
    bizType: string | null;
    servicePackageName: string | null;
    createdAt: string | null;
    userType: string | null;
}

}
export namespace HospitioOnBoarding.GetHospitioOnBoarding {
export interface GetHospitioOnBoardingIn {

}

}
export namespace HospitioOnBoarding.GetHospitioOnBoarding {
export interface GetHospitioOnBoardingOut extends BaseResponseOut {
    hospitioOnBoardingOut: HospitioOnBoardingOut;
}

export interface HospitioOnBoardingOut {
    id: number;
    whatsappCountry: string | null;
    whatsappNumber: string | null;
    viberCountry: string | null;
    viberNumber: string | null;
    telegramCountry: string | null;
    telegramNumber: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    smsTitle: string | null;
    messenger: string | null;
    email: string | null;
    cname: string | null;
    incomingTranslationLanguage: string | null;
    noTranslateWords: string | null;
}

}
export namespace HospitioOnBoarding.UpdateHospitioOnBoardings {
export interface UpdateHospitioOnBoardingsIn {
    id: number;
    whatsappCountry: string | null;
    whatsappNumber: string | null;
    viberCountry: string | null;
    viberNumber: string | null;
    telegramCountry: string | null;
    telegramNumber: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    smsTitle: string | null;
    messenger: string | null;
    email: string | null;
    cname: string | null;
    incomingTranslationLanguage: string | null;
    noTranslateWords: string | null;
}

}
export namespace HospitioOnBoarding.UpdateHospitioOnBoardings {
export interface UpdateHospitioOnBoardingsOut extends BaseResponseOut {
    updatedHospitioOnBoardingsOut: UpdatedHospitioOnBoardingsOut;
}

export interface UpdatedHospitioOnBoardingsOut {
    id: number;
}

}
export namespace HospitioPaymentProcessors.CreateHospitioPaymentProcessors {
export interface CreateHospitioPaymentProcessorsIn {
    paymentProcessorId: number;
    clientId: string;
    clientSecret: string;
    currency: string;
}

}
export namespace HospitioPaymentProcessors.CreateHospitioPaymentProcessors {
export interface CreateHospitioPaymentProcessorsOut extends BaseResponseOut {
    createdHospitioPaymentProcessorsOut: CreatedHospitioPaymentProcessorsOut;
}

export interface CreatedHospitioPaymentProcessorsOut {
    id: number;
    paymentProcessorId: number | null;
    clientId: string;
    clientSecret: string;
    currency: string;
}

}
export namespace HospitioPaymentProcessors.DeleteHospitioPaymentProcessors {
export interface DeleteHospitioPaymentProcessorsIn {
    id: number;
}

}
export namespace HospitioPaymentProcessors.DeleteHospitioPaymentProcessors {
export interface DeleteHospitioPaymentProcessorsOut extends BaseResponseOut {
    deletedHospitioPaymentProcessorsOut: DeletedHospitioPaymentProcessorsOut;
}

export interface DeletedHospitioPaymentProcessorsOut {
    id: number;
    clientId: string;
}

}
export namespace HospitioPaymentProcessors.GetHospitioPaymentProcessorById {
export interface GetHospitioPaymentProcessorByIdIn {
    id: number;
}

}
export namespace HospitioPaymentProcessors.GetHospitioPaymentProcessorById {
export interface GetHospitioPaymentProcessorByIdOut extends BaseResponseOut {
    hospitioPaymentProcessorByIdOut: HospitioPaymentProcessorByIdOut;
}

export interface HospitioPaymentProcessorByIdOut {
    id: number;
    paymentProcessorId: number | null;
    clientId: string;
    clientSecret: string;
    currency: string;
}

}
export namespace HospitioPaymentProcessors.GetHospitioPaymentProcessors {
export interface GetHospitioPaymentProcessorsIn {
    pageNo: number;
    pageSize: number;
}

}
export namespace HospitioPaymentProcessors.GetHospitioPaymentProcessors {
export interface GetHospitioPaymentProcessorsOut extends BaseResponseOut {
    hospitioPaymentProcessorsOut: HospitioPaymentProcessorsOut[];
}

export interface HospitioPaymentProcessorsOut {
    id: number;
    paymentProcessorId: number | null;
    clientId: string;
    clientSecret: string;
    currency: string;
}

}
export namespace HospitioPaymentProcessors.UpdateHospitioPaymentProcessors {
export interface UpdateHospitioPaymentProcessorsIn {
    id: number;
    paymentProcessorId: number;
    clientId: string;
    clientSecret: string;
    currency: string;
}

}
export namespace HospitioPaymentProcessors.UpdateHospitioPaymentProcessors {
export interface UpdateHospitioPaymentProcessorsOut extends BaseResponseOut {
    updatedHospitioPaymentProcessorsOut: UpdatedHospitioPaymentProcessorsOut;
}

export interface UpdatedHospitioPaymentProcessorsOut {
    id: number;
    paymentProcessorId: number;
    clientId: string;
    clientSecret: string;
    currency: string;
}

}
export namespace Leads.CreateLead {
export interface CreateLeadIn {
    id: number;
    firstName: string | null;
    lastName: string | null;
    company: string | null;
    email: string | null;
    comment: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    contactFor: number | null;
    isActive: boolean | null;
}

}
export namespace Leads.CreateLead {
export interface CreateLeadOut extends BaseResponseOut {
    createdLeadOut: CreatedLeadOut;
}

export interface CreatedLeadOut {
    id: number;
    firstName: string | null;
    lastName: string | null;
    company: string | null;
    email: string | null;
    comment: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    contactFor: number | null;
    isActive: boolean | null;
}

}
export namespace Leads.DeleteLead {
export interface DeleteLeadIn {
    leadId: number;
}

}
export namespace Leads.DeleteLead {
export interface DeleteLeadOut extends BaseResponseOut {
    removeLeadOut: RemoveLeadOut;
}

export interface RemoveLeadOut {
    deletedLeadId: number;
}

}
export namespace Leads.DownloadLead {
export interface DownloadLeadIn {

}

}
export namespace Leads.DownloadLead {
export interface DownloadLeadOut extends BaseResponseOut {
    downloadLeadByOut: string;
}

export interface DownloadLeadByOut {
    name: string | null;
    company: string | null;
    phoneNumber: string | null;
    email: string | null;
    createdAt: string | null;
    comment: string | null;
}

}
export namespace Leads.EditLead {
export interface EditLeadIn {
    id: number;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    comment: string | null;
    company: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    contactFor: number | null;
    isActive: boolean | null;
}

}
export namespace Leads.EditLead {
export interface EditLeadOut extends BaseResponseOut {
    editedLeadOut: EditedLeadOut;
}

export interface EditedLeadOut {
    id: number;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    company: string | null;
    comment: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    contactFor: number | null;
    isActive: boolean | null;
}

}
export namespace Leads.GetLeadById {
export interface GetLeadByIdIn {
    id: number;
}

}
export namespace Leads.GetLeadById {
export interface GetLeadByIdOut extends BaseResponseOut {
    leadByIdOut: LeadByIdOut;
}

export interface LeadByIdOut {
    id: number;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    company: string | null;
    comment: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    contactFor: number | null;
    isActive: boolean | null;
}

}
export namespace Leads.GetLeads {
export interface GetLeadsIn extends BaseSearchFilterOptions {
    alphabetsStartsWith: string | null;
}

}
export namespace Leads.GetLeads {
export interface GetLeadsOut extends BaseResponseOut {
    leadsOuts: LeadsOut[];
}

export interface LeadsOut {
    id: number;
    name: string | null;
    email: string | null;
    comment: string | null;
    phoneNumber: string | null;
    contactFor: number | null;
    isActive: boolean | null;
    createdAt: string | null;
    updateAt: string | null;
    company: string | null;
    filteredCount: string | null;
}

}
export namespace Modules.GetModules {
export interface GetModulesIn {

}

}
export namespace Modules.GetModules {
export interface GetModulesOut extends BaseResponseOut {
    modulesOut: ModulesOut[];
}

export interface ModulesOut {
    id: number;
    name: string;
    moduleType: number | null;
}

}
export namespace Modules.GetModulesDapperDemo {
export interface GetModulesDapperDemoIn {
    id: number | null;
    pageNo: number;
    pageSize: number;
}

}
export namespace Modules.GetModulesDapperDemo {
export interface GetModulesDapperOut extends BaseResponseOut {
    modulesOut: ModulesDapperOut[];
}

export interface ModulesDapperOut {
    id: number;
    name: string;
    moduleType: number | null;
    isActive: boolean;
    createdAt: string;
    totalCount: number;
}

}
export namespace ModuleServices.GetModuleServiceById {
export interface GetModuleServiceByIdIn {
    id: number;
}

}
export namespace ModuleServices.GetModuleServiceById {
export interface GetModuleServiceByIdOut extends BaseResponseOut {
    moduleServiceById: ModuleServiceById;
}

export interface ModuleServiceById {
    id: number;
    name: string | null;
    moduleId: number | null;
}

}
export namespace ModuleServices.GetModuleServices {
export interface GetModuleServices {

}

}
export namespace ModuleServices.GetModuleServices {
export interface GetModuleServicesOut extends BaseResponseOut {
    moduleServicesOut: ModuleServicesOut[];
}

export interface ModuleServicesOut {
    id: number;
    name: string;
    moduleId: number;
}

}
export namespace Musement.CancelMusementItem {
export interface CancelMusementItemIn {
    itemUUID: string | null;
    orderUUID: string | null;
}

}
export namespace Musement.CancelMusementItem {
export interface CancelMusementItemOut extends BaseResponseOut {

}

}
export namespace Musement.DeleteMusementCartById {
export interface DeleteMusementCartById {
    cartId: string | null;
}

}
export namespace Musement.DeleteMusementCartById {
export interface DeleteMusementCartByIdOut extends BaseResponseOut {

}

}
export namespace Musement.DownloadMusementData {
export interface DownloadMusementDataIn {

}

}
export namespace Musement.DownloadMusementData {
export interface DownloadMusementDataOut extends BaseResponseOut {
    downloadMusementData: DownloadMusementData[] | null;
}

export interface DownloadMusementData {
    businessName: string | null;
    reservationNumber: string | null;
    city: string | null;
    firstname: string | null;
    lastname: string | null;
    orderId: string | null;
    paymentDate: string | null;
    musementItemInfo: MusementItemnInfo[] | null;
    amount: number | null;
    currency: string | null;
    musementCancelItemInfo: MusementItemnInfo[] | null;
}

export interface MusementItemnInfo {
    title: string | null;
}

}
export namespace Musement.GetMusementData {
export interface GetMusementDataIn {
    searchValue: string | null;
    pageNo: number | null;
    pageSize: number | null;
}

}
export namespace Musement.GetMusementData {
export interface GetMusementDataOut extends BaseResponseOut {
    getMusementDataResponseOut: GetMusementDataResponseOut[] | null;
}

export interface GetMusementDataResponseOut {
    businessName: string | null;
    reservationNumber: string | null;
    city: string | null;
    country: string | null;
    firstname: string | null;
    lastname: string | null;
    amount: number | null;
    currency: string | null;
    orderId: string | null;
    paymentMethod: string | null;
    paymentDate: string | null;
    filterCount: number | null;
    musementItemInfo: MusementItemnInfo[] | null;
    musementCancelItemInfo: MusementItemnInfo[] | null;
}

export interface MusementItemnInfo {
    title: string | null;
    ticketHolder: string | null;
    isCancel: boolean | null;
    itemPrice: number | null;
}

}
export namespace Musement.GetMusementOrderIdByCartId {
export interface GetMusementOrderIdByCartIdIn {
    cartId: string | null;
}

}
export namespace Musement.GetMusementOrderIdByCartId {
export interface GetMusementOrderIdByCartIdOut extends BaseResponseOut {
    getMusementOrderIdByCartIdOutResponse: GetMusementOrderIdByCartIdOutResponse | null;
}

export interface GetMusementOrderIdByCartIdOutResponse {
    orderId: string | null;
    orderDate: string | null;
}

}
export namespace Musement.GetMusementOrderIds {
export interface GetMusementOrderIdsIn {
    guestId: string | null;
    customerId: string | null;
}

}
export namespace Musement.GetMusementOrderIds {
export interface GetMusementOrderIdsOut extends BaseResponseOut {
    getMusementOrderIdsOutResponse: GetMusementOrderIdsOutResponse;
}

export interface GetMusementOrderIdsOutResponse {
    orderId: string[] | null;
}

}
export namespace Musement.MusementBeginPayment {
export interface MusementBeginPaymentIn {
    url: string;
    adyen_token: string;
    card_brand: string;
    card_country: string;
    client_ip: string;
    order_uuid: string;
    redirect_url_success_3d_secure: string;
}

}
export namespace Musement.MusementBeginPayment {
export interface MusementBeginPaymentOut extends BaseResponseOut {
    musementBeginPaymentResponse: string;
}

export interface musementBeginPaymentClass {
    gateway: string | null;
    threeDSecure: ThreeDSecure | null;
    reason: string | null;
}

export interface ThreeDSecure {
    payload: any | null;
    payment_intent_client_secret: string | null;
    type: string | null;
    url: string | null;
}

}
export namespace Musement.MusementCompletePayment {
export interface MusementCompletePaymentIn {
    order_uuid: string | null;
    guestId: string | null;
    customerId: string | null;
    paymentMethod: string | null;
}

}
export namespace Musement.MusementCompletePayment {
export interface MusementCompletePaymentOut extends BaseResponseOut {

}

}
export namespace Musement.MusementCreateCart {
export interface MusementCreateCartIn {
    guestId: string | null;
    cartid: string | null;
}

}
export namespace Musement.MusementCreateCart {
export interface MusementCreateCartOut extends BaseResponseOut {

}

}
export namespace Musement.MusementCreateOrder {
export interface MusementCreateOrderIn {
    url: string | null;
    cart_uuid: string | null;
    email_notification: string | null;
    guest_Id: string | null;
    order_uuid: string | null;
    extra_data: ExtraData | null;
}

export interface ExtraData {
    clientReferenceId: string | null;
    firstName: string | null;
    lastName: string | null;
    reservationId: string | null;
    utm_campaign: string | null;
    utm_content: string | null;
    utm_medium: string | null;
    utm_source: string | null;
}

}
export namespace Musement.MusementCreateOrder {
export interface MusementCreateOrderOut extends BaseResponseOut {
    musementCreateOrderResponse: string;
}

export interface order {
    identifier: string | null;
    uuid: string;
    status: string | null;
    trustpilot_url: string | null;
    customer: customer | null;
    items: orderitem[] | null;
    total_price: totalprice | null;
    discount_amount: discountamount | null;
    extra_data: string | null;
    market: string | null;
    is_agency: boolean | null;
    is_paid: boolean | null;
}

export interface customer {
    id: number | null;
    email: string | null;
    firstname: string | null;
    lastname: string | null;
}

export interface orderitem {
    uuid: string | null;
    cart_item_uuid: string | null;
    transaction_code: string | null;
    product: product | null;
    quantity: number | null;
    retail_price_in_order_currency: totalprice | null;
    total_retail_price_in_order_currency: totalprice | null;
    status: string | null;
    vouchers: voucher[] | null;
    is_error_status: boolean | null;
}

export interface product {
    type: string | null;
    max_confirmation_time: string | null;
    price_tag: pricetag | null;
    id: string | null;
    title: string | null;
    activity_uuid: string | null;
    api_url: string | null;
    url: string | null;
    cover_image_url: string | null;
    original_retail_price: totalprice | null;
    original_retail_price_without_service_fee: totalprice | null;
    retail_price: totalprice | null;
    retail_price_without_service_fee: totalprice | null;
    discount_amount: discountamount | null;
    service_fee: servicefee | null;
    meeting_point: string | null;
    meeting_point_markdown: string | null;
    meeting_point_html: string | null;
}

export interface pricetag {
    price_feature: string | null;
    ticket_holder: string | null;
    price_feature_code: string | null;
    ticket_holder_code: string | null;
}

export interface totalprice {
    currency: string | null;
    value: number | null;
    formatted_value: string | null;
    formatted_iso_value: string | null;
}

export interface discountamount {
    currency: string | null;
    value: number | null;
    formatted_value: string | null;
    formatted_iso_value: string | null;
}

export interface voucher {
    uuid: string | null;
    code: string | null;
    discountAmount: discountamount | null;
    status: string | null;
    type: string | null;
}

export interface servicefee {
    currency: string | null;
    value: number | null;
    formatted_value: string | null;
    formatted_iso_value: string | null;
}

}
export namespace Musement.MusementGetCartId {
export interface MusementGetCartIdIn {
    guestId: number;
}

}
export namespace Musement.MusementGetCartId {
export interface MusementGetCartIdOut extends BaseResponseOut {
    musementGetCartIdResponse: MusementGetCartIdResponse | null;
}

export interface MusementGetCartIdResponse {
    cartId: string | null;
}

}
export namespace Musement.MusementLogin {
export interface MusementLoginIn {

}

}
export namespace Musement.MusementLogin {
export interface MusementLoginOut extends BaseResponseOut {
    musementLoginResponseOut: MusementLoginResponseOut;
}

export interface MusementLoginResponseOut {
    access_token: string | null;
    expires_in: number | null;
    token_type: string | null;
    scope: string | null;
}

}
export namespace Musement.MusementNoPaymentFlow {
export interface MusementNoPaymentFlowIn {
    url: string;
    uuid: string;
    token: string;
}

}
export namespace Musement.MusementNoPaymentFlow {
export interface MusementNoPaymentFlowOut extends BaseResponseOut {
    musementNoPaymentFlowOutResponse: string;
}

export interface Root {
    customer: Customer | null;
    date: string | null;
    discount_amount: DiscountAmount | null;
    extra_data: string | null;
    identifier: string | null;
    items: Item[] | null;
    market: string | null;
    status: string | null;
    total_price: DiscountAmount | null;
    trustpilot_url: string | null;
    uuid: string | null;
    total_retail_price_in_order_currency: DiscountAmount | null;
    total_supplier_original_retail_price_in_supplier_currency: DiscountAmount | null;
    total_supplier_price_in_supplier_currency: DiscountAmount | null;
    affiliate: Affiliate | null;
    affiliate_channel: string | null;
    promo_codes: PromoCode[] | null;
    source: string | null;
}

export interface Customer {
    email: string | null;
    events_related_newsletter: string | null;
    extra_customer_data: { [key: string]: ExtraCustomerData; } | null;
    firstname: string | null;
    lastname: string | null;
    musement_newsletter: string | null;
    thirdparty_newsletter: string | null;
}

export interface DiscountAmount {
    currency: string | null;
    formatted_value: string | null;
    formatted_iso_value: string | null;
    value: number;
}

export interface Item {
    quantity: number;
    b2b_price: DiscountAmount | null;
    cancellation_additional_info: string | null;
    cancellation_reason: string | null;
    error_status: boolean;
    extra_customer_data: ExtraCustomerData[] | null;
    is_gift_redeem: boolean;
    participants_info: ParticipantsInfo[] | null;
    product: Product | null;
    status: string | null;
    transactionCode: string | null;
    uuid: string | null;
    vouchers: Voucher[] | null;
    retail_price_in_order_currency: DiscountAmount | null;
    total_retail_price_in_order_currency: DiscountAmount | null;
    original_retail_price_in_supplier_currency: DiscountAmount | null;
    total_original_retail_price_in_supplier_currency: DiscountAmount | null;
}

export interface ExtraCustomerData {
    name: number | null;
}

export interface ParticipantsInfo {
    salutation: string | null;
    firstname: string | null;
    lastname: string | null;
    date_of_birth: string | null;
    passport: string | null;
    email: string | null;
    passport_expiry_date: string | null;
    nationality: string | null;
    medicalNotes: string | null;
    address: string | null;
    fan_card: string | null;
    weight: number;
    phoneNumber: string | null;
}

export interface Product {
    activity_uuid: string | null;
    api_url: string | null;
    cover_image_url: string | null;
    date: string | null;
    discount_amount: DiscountAmount | null;
    id: string | null;
    language: Language | null;
    max_confirmation_time: string | null;
    original_retail_price: OriginalRetailPrice | null;
    original_retail_price_without_service_fee: OriginalRetailPrice | null;
    retail_price: DiscountAmount | null;
    retail_price_without_service_fee: DiscountAmount | null;
    service_fee: DiscountAmount | null;
    title: string | null;
    type: string | null;
    url: string | null;
}

export interface Language {
    code: string | null;
    name: string | null;
}

export interface OriginalRetailPrice {
    currency: string | null;
    formattedValue: string | null;
    formattedIsoValue: string | null;
    value: number;
}

export interface Voucher {
    url: string | null;
}

export interface PromoCode {
    code: string | null;
    active: boolean;
    percentage: boolean;
    discount: number;
    max_usage: number;
    valid_from: string | null;
    valid_until: string | null;
    minimum_amount: number | null;
}

export interface Affiliate {
    uuid: string | null;
    email: string | null;
    first_name: string | null;
    last_name: string | null;
    code: string | null;
    name: string | null;
    logoUrl: string | null;
    secondary_logo_url: string | null;
    header: string | null;
    customer_care_phone_number: string | null;
    customer_care_email: string | null;
    whitelabel: boolean;
    show_cobranded_header: boolean;
    show_cobranded_voucher: boolean;
    show_cobranded_item_confirmation_email: boolean;
    setup_cookie_after_first_visit: boolean;
    translations: Translation[] | null;
}

export interface Translation {
    locale: string | null;
}

}
export namespace Notifications.CreateNotifications {
export interface CreateNotificationsIn {
    country: string | null;
    city: string | null;
    postalcode: string | null;
    businessTypeId: number | null;
    productId: number | null;
    title: string | null;
    message: string | null;
    customerId: number | null;
    currentUserType: number | null;
    userId: number | null;
}

}
export namespace Notifications.CreateNotifications {
export interface CreateNotificationsOut extends BaseResponseOut {
    createdNotificationsOut: CreatedNotificationsOut;
}

export interface CreatedNotificationsOut {
    id: number;
    country: string | null;
    city: string | null;
    postalcode: string | null;
    businessTypeId: number | null;
    productId: number | null;
    title: string | null;
    message: string | null;
}

export interface UserNotification {
    userId: number;
    isActive: number;
}

}
export namespace Notifications.GetNotifications {
export interface GetNotificationsIn {
    pageNo: number | null;
    pageSize: number | null;
    userId: number | null;
    userType: number | null;
}

}
export namespace Notifications.GetNotifications {
export interface GetNotificationsOut extends BaseResponseOut {
    notificationOut: NotificationOut[];
}

export interface NotificationOut {
    id: number;
    title: string | null;
    message: string | null;
    createdAt: string | null;
    filteredCount: number | null;
    messageType: string | null;
}

}
export namespace PaymentDetails.GetAdminPaymentDetail {
export interface GetAdminPaymentDetailIn {
    customerId: number | null;
    guestId: number;
}

}
export namespace PaymentDetails.GetAdminPaymentDetail {
export interface GetAdminPaymentDetailOut extends BaseResponseOut {
    adminPaymentDetailOut: AdminPaymentDetailOut;
}

export interface AdminPaymentDetailOut {
    token: string | null;
    merchant_Account_Id: string | null;
    buyer_Id: string | null;
    country: string | null;
}

}
export namespace PaymentDetails.GetPaymentDetail {
export interface GetPaymentDetailIn {
    customerId: number;
    guestId: number;
}

}
export namespace PaymentDetails.GetPaymentDetail {
export interface GetPaymentDetailOut extends BaseResponseOut {
    paymentDetailOut: PaymentDetailOut;
}

export interface PaymentDetailOut {
    token: string | null;
    merchant_Account_Id: string | null;
    buyer_Id: string | null;
    country: string | null;
}

}
export namespace PaymentProcessors.CreatePaymentProcessors {
export interface CreatePaymentProcessorsIn {
    name: string;
}

}
export namespace PaymentProcessors.CreatePaymentProcessors {
export interface CreatePaymentProcessorsOut extends BaseResponseOut {
    createdPaymentProcessorsOut: CreatedPaymentProcessorsOut;
}

export interface CreatedPaymentProcessorsOut {
    id: number;
    name: string;
}

}
export namespace PaymentProcessors.DeletePaymentProcessors {
export interface DeletePaymentProcessorsIn {
    id: number;
}

}
export namespace PaymentProcessors.DeletePaymentProcessors {
export interface DeletePaymentProcessorsOut extends BaseResponseOut {
    deletedPaymentProcessorsOut: DeletedPaymentProcessorsOut;
}

export interface DeletedPaymentProcessorsOut {
    id: number;
}

}
export namespace PaymentProcessors.GetPaymentProcessors {
export interface GetPaymentProcessorsIn {

}

}
export namespace PaymentProcessors.GetPaymentProcessors {
export interface GetPaymentProcessorsOut extends BaseResponseOut {
    paymentProcessorsOut: PaymentProcessorsOut[];
}

export interface PaymentProcessorsOut {
    id: number;
    gRCategory: string;
    gRGroup: string;
    gRID: string;
    gRIcon: string;
    gRName: string;
}

}
export namespace PaymentProcessors.GetPaymentProcessorsById {
export interface GetPaymentProcessorsByIdIn {
    id: number;
}

}
export namespace PaymentProcessors.GetPaymentProcessorsById {
export interface GetPaymentProcessorsByIdOut extends BaseResponseOut {
    paymentProcessorsByIdOut: PaymentProcessorsByIdOut;
}

export interface PaymentProcessorsByIdOut {
    id: number;
    name: string;
}

}
export namespace PaymentProcessors.UpdatePaymentProcessors {
export interface UpdatePaymentProcessorsIn {
    id: number;
    name: string;
}

}
export namespace PaymentProcessors.UpdatePaymentProcessors {
export interface UpdatePaymentProcessorsOut extends BaseResponseOut {
    updatedPaymentProcessorsOut: UpdatedPaymentProcessorsOut;
}

export interface UpdatedPaymentProcessorsOut {
    id: number;
    name: string;
}

}
export namespace PaymentProcessorsDefinations.GetPaymentProcessorsDefinationsByPaymentProcessorsId {
export interface GetPaymentProcessorsDefinationsByPaymentProcessorsIdIn {
    paymentProcessorId: number;
}

}
export namespace PaymentProcessorsDefinations.GetPaymentProcessorsDefinationsByPaymentProcessorsId {
export interface GetPaymentProcessorsDefinationsByPaymentProcessorsIdOut extends BaseResponseOut {
    _PaymentProcessorsDefinationsByPaymentProcessorsIdOut: PaymentProcessorsDefinationsByPaymentProcessorsIdOut;
}

export interface PaymentProcessorsDefinationsByPaymentProcessorsIdOut {
    gRFields: string | null;
    gRSupportedCountries: string | null;
    gRSupportedCurrencies: string | null;
    gRSupportedFeatures: string | null;
    paymentProcessorId: number;
}

}
export namespace PaymentServiceDefinitions.SyncPaymentServiceDefinitions {
export interface SyncPaymentServiceDefinitionsIn {
    item: Gr4vyConnectionDefinitionModel;
}

export interface Gr4vyConnectionDefinitionModel {
    id: string | null;
    type: string | null;
    display_name: string | null;
    method: string | null;
    fields: FieldModel[];
    supported_currencies: string[];
    supported_countries: string[];
    mode: string | null;
    icon_url: string | null;
    supported_features: SupportedFeaturesModel;
    configuration: ConfigurationModel;
}

export interface FieldModel {
    key: string | null;
    display_name: string | null;
    required: boolean;
    format: string | null;
    secret: boolean;
}

export interface SupportedFeaturesModel {
    delayed_capture: boolean;
    network_tokens: boolean;
    network_tokens_default: boolean;
    network_tokens_toggle: boolean;
    open_loop: boolean;
    open_loop_toggle: boolean;
    partial_capture: boolean;
    partial_refunds: boolean;
    payment_method_tokenization: boolean;
    payment_method_tokenization_toggle: boolean;
    refunds: boolean;
    requires_webhook_setup: boolean;
    three_d_secure_hosted: boolean;
    three_d_secure_pass_through: boolean;
    verify_credentials: boolean;
    void: boolean;
    zero_auth: boolean;
}

export interface ConfigurationModel {
    approval_ui_target: string | null;
    approval_ui_height: string | null;
    approval_ui_width: string | null;
    cart_items_limit: number;
    cart_items_required: boolean;
    cart_items_should_match_amount: boolean;
}

}
export namespace PaymentServiceDefinitions.SyncPaymentServiceDefinitions {
export interface SyncPaymentServiceDefinitionsOut extends BaseResponseOut {

}

}
export namespace PMS.GetPMS {
export interface GetPMSIn {

}

}
export namespace PMS.GetPMS {
export interface GetPMSOut extends BaseResponseOut {
    pMSOut: PMSOut[];
}

export interface PMSOut {
    id: number;
    pMS: string;
}

}
export namespace Product.CreateProduct {
export interface CreateProductIn {
    productId: number;
    productName: string;
    isActive: boolean | null;
    productModules: ProductModuleRequest[] | null;
}

export interface ProductModuleRequest {
    id: number;
    moduleId: number;
    moduleType: number | null;
    module2TypeValue: string | null;
    price: number | null;
    currency: string | null;
    isActive: boolean;
    productModuleServices: ProductModuleService[];
}

export interface ProductModuleService {
    id: number;
    productModuleId: number | null;
    productId: number | null;
    moduleServiceId: number | null;
    isActive: boolean;
}

}
export namespace Product.CreateProduct {
export interface CreateProductOut extends BaseResponseOut {
    createdProductOut: CreatedProductOut;
}

export interface CreatedProductOut {
    id: number;
    name: string | null;
}

}
export namespace Product.EditProduct {
export interface EditProductIn {
    name: string;
}

}
export namespace Product.EditProduct {
export interface EditProductOut extends BaseResponseOut {

}

}
export namespace Product.GetProductById {
export interface GetProductByIdIn {

}

}
export namespace Product.GetProductById {
export interface GetProductByIdOut extends BaseResponseOut {
    getProductByIdResponseOut: GetProductByIdResponseOut;
}

export interface GetProductByIdResponseOut {
    productId: number | null;
    productName: string | null;
    isActive: boolean | null;
    createdAt: string | null;
    updatedAt: string | null;
    createdBy: string;
    productModules: ProductModule[];
}

export interface ProductModule {
    id: number;
    productId: number | null;
    moduleId: number | null;
    moduleName: string | null;
    module2TypeValue: string | null;
    moduleType: number | null;
    price: number | null;
    currency: string | null;
    isActive: boolean;
    productModuleServices: ProductModuleService[];
}

export interface ProductModuleService {
    id: number;
    productModuleId: number | null;
    productId: number | null;
    moduleServiceId: number | null;
    moduleServiceName: string | null;
    isActive: boolean;
}

}
export namespace Product.GetProducts {
export interface GetProductsIn {
    searchValue: string;
    pageNo: number | null;
    pageSize: number | null;
    sortColumn: string | null;
    sortOrder: string | null;
}

}
export namespace Product.GetProducts {
export interface GetProductsOut extends BaseResponseOut {
    getProductsResponseOut: GetProductsResponseOut[];
}

export interface GetProductsResponseOut {
    id: number;
    name: string | null;
    createdAt: string | null;
    createdBy: string | null;
    isActive: boolean | null;
    totalCount: number | null;
}

}
export namespace ProductModules.CreateProductModule {
export interface CreateProductModuleIn {
    productId: number;
    moduleId: number;
    price: number;
    currency: string | null;
}

}
export namespace ProductModules.CreateProductModule {
export interface CreateProductModuleOut extends BaseResponseOut {

}

}
export namespace ProductModules.EditProductModule {
export interface EditProductModuleIn {
    productId: number;
    moduleId: number;
    price: number;
    currency: string | null;
}

}
export namespace ProductModules.EditProductModule {
export interface EditProductModuleOut extends BaseResponseOut {

}

}
export namespace ProductModules.GetProductModuleById {
export interface GetProductModuleByIdIn {

}

}
export namespace ProductModules.GetProductModuleById {
export interface GetProductModuleByIdOut extends BaseResponseOut {
    getProductModuleByIdResponseOut: GetProductModuleByIdResponseOut;
}

export interface GetProductModuleByIdResponseOut {
    id: number;
    productId: number | null;
    moduleId: number | null;
    price: number;
    currency: string | null;
    productName: string | null;
    moduleName: string | null;
}

}
export namespace ProductModules.GetProductModules {
export interface GetProductModulesIn {

}

}
export namespace ProductModules.GetProductModules {
export interface GetProductModulesOut extends BaseResponseOut {
    getProductModulesResponseOut: GetProductModulesResponseOut[];
}

export interface GetProductModulesResponseOut {
    id: number;
    productId: number | null;
    moduleId: number | null;
    price: number;
    currency: string | null;
    productName: string | null;
    moduleName: string | null;
}

}
export namespace ProductModuleServices.CreateProductModuleService {
export interface CreateProductModuleServiceIn {
    productModuleId: number | null;
    productId: number | null;
    moduleServiceId: number | null;
}

}
export namespace ProductModuleServices.CreateProductModuleService {
export interface CreateProductModuleServiceOut extends BaseResponseOut {

}

}
export namespace ProductModuleServices.EditProductModuleService {
export interface EditProductModuleServiceIn {
    productModuleId: number | null;
    productId: number | null;
    moduleServiceId: number | null;
}

}
export namespace ProductModuleServices.EditProductModuleService {
export interface EditProductModuleServiceOut extends BaseResponseOut {

}

}
export namespace ProductModuleServices.GetProductModuleServiceById {
export interface GetProductModuleServiceByIdIn {

}

}
export namespace ProductModuleServices.GetProductModuleServiceById {
export interface GetProductModuleServiceByIdOut extends BaseResponseOut {
    getProductModuleServiceByIdResponseOut: GetProductModuleServiceByIdResponseOut;
}

export interface GetProductModuleServiceByIdResponseOut {
    id: number;
    productModuleId: number | null;
    productId: number | null;
    productName: string | null;
    moduleServiceId: number | null;
    moduleServiceName: string | null;
}

}
export namespace ProductModuleServices.GetProductModuleServices {
export interface GetProductModuleServicesIn {

}

}
export namespace ProductModuleServices.GetProductModuleServices {
export interface GetProductModuleServicesOut extends BaseResponseOut {
    getProductModuleServicesResponseOut: GetProductModuleServicesResponseOut[];
}

export interface GetProductModuleServicesResponseOut {
    id: number;
    productModuleId: number | null;
    productId: number | null;
    productName: string | null;
    moduleServiceId: number | null;
    moduleServiceName: string | null;
}

}
export namespace ProductNames.GetProductNames {
export interface GetProductNamesIn {

}

}
export namespace ProductNames.GetProductNames {
export interface GetProductNamesOut extends BaseResponseOut {
    productNamesOuts: ProductNamesOut[];
}

export interface ProductNamesOut {
    id: number;
    name: string;
}

}
export namespace PropertyGallery.CreatePropertyGallery {
export interface CreatePropertyGalleryIn {
    createPropertyGalleryImagesIns: CreatePropertyGalleryImagesIn[];
}

export interface CreatePropertyGalleryImagesIn {
    id: number;
    customerPropertyInformationId: number;
    isActive: boolean | null;
    propertyImage: string | null;
    isDeleted: boolean;
}

}
export namespace PropertyGallery.CreatePropertyGallery {
export interface CreatePropertyGalleryOut extends BaseResponseOut {

}

export interface CreatedPropertyGalleryOut {
    id: number;
    customerPropertyInformationId: number | null;
    isActive: boolean | null;
    propertyImage: string | null;
}

}
export namespace PropertyGallery.EditPropertyGallery {
export interface EditPropertyGalleryIn {
    id: number;
    customerPropertyInformationId: number;
    isActive: boolean;
    propertyImages: string | null;
}

}
export namespace PropertyGallery.EditPropertyGallery {
export interface EditPropertyGalleryOut extends BaseResponseOut {

}

}
export namespace PropertyGallery.GetPropertyFile {
export interface GetPropertyFileIn {
    id: number;
    customerPropertyInformationId: number;
}

}
export namespace PropertyGallery.GetPropertyFile {
export interface GetPropertyFileOut extends BaseResponseOut {
    memoryStream: any;
    fileName: string;
    imageId: number;
    contentType: string;
}

}
export namespace PropertyGallery.GetPropertyGallery {
export interface GetPropertyGalleryIn extends BaseSearchFilterOptions {
    customerPropertyInformationId: number;
}

}
export namespace PropertyGallery.GetPropertyGallery {
export interface GetPropertyGalleryOut extends BaseResponseOut {
    propertyGallery: PropertyGalleryOut[];
}

export interface PropertyGalleryOut {
    id: number;
    customerPropertyInformationId: number;
    propertyImage: string;
    isDeleted: boolean;
}

}
export namespace QaCategories.CreateQaCategory {
export interface CreateQaCategoryIn {
    name: string;
    isActive: boolean;
}

}
export namespace QaCategories.CreateQaCategory {
export interface CreateQaCategoryOut extends BaseResponseOut {
    createdQaCategoryOut: CreatedQaCategoryOut;
}

export interface CreatedQaCategoryOut {
    id: number;
    name: string;
}

}
export namespace QaCategories.DeleteQaCategory {
export interface DeleteQaCategoryIn {
    qaCategoryId: number;
}

}
export namespace QaCategories.DeleteQaCategory {
export interface DeleteQaCategoryOut extends BaseResponseOut {
    deletedQaCategory: DeletedQaCategory;
}

export interface DeletedQaCategory {
    qaCategoryId: number;
}

}
export namespace QaCategories.GetQaCategories {
export interface GetQaCategoriesOut extends BaseResponseOut {
    qaCategories: QaCategoriesOut[];
}

export interface QaCategoriesOut {
    id: number;
    name: string;
}

}
export namespace QaCategories.GetQaCategory {
export interface GetQaCategoryIn {
    id: number;
}

}
export namespace QaCategories.GetQaCategory {
export interface GetQaCategoryOut extends BaseResponseOut {
    qaCategoryOut: QaCategoryOut;
}

export interface QaCategoryOut {
    id: number;
    name: string;
}

}
export namespace QaCategories.UpdateQaCategory {
export interface UpdateQaCategoryIn {
    id: number;
    name: string;
}

}
export namespace QaCategories.UpdateQaCategory {
export interface UpdateQaCategoryOut extends BaseResponseOut {
    updatedQaCategoryOut: UpdatedQaCategoryOut;
}

export interface UpdatedQaCategoryOut {
    id: number;
    name: string;
}

}
export namespace QuestionAnswer.CreateQuestionAnswer {
export interface CreateQuestionAnswerIn {
    id: number;
    questionAnswerCategoryId: number;
    name: string;
    description: string;
    icon: string;
    isActive: boolean;
    isPublish: boolean;
    questionAnswerAttachements: QuestionAnswerAttachementIn[];
}

export interface QuestionAnswerAttachementIn {
    questionAnswerId: number | null;
    attachment: string | null;
    attachmentType: string | null;
}

}
export namespace QuestionAnswer.CreateQuestionAnswer {
export interface CreateQuestionAnswerOut extends BaseResponseOut {
    createdQuestionAnswerOut: CreatedQuestionAnswerOut;
}

export interface CreatedQuestionAnswerOut {
    id: number;
}

}
export namespace QuestionAnswer.DeleteQuestionAnswer {
export interface DeleteQuestionAnswerIn {
    questionAnswerId: number;
}

}
export namespace QuestionAnswer.DeleteQuestionAnswer {
export interface DeleteQuestionAnswerOut extends BaseResponseOut {
    removeQuestionAnswerOut: RemoveQuestionAnswerOut;
}

export interface RemoveQuestionAnswerOut {
    deletedQuestionAnswerId: number;
}

}
export namespace QuestionAnswer.EditQuestionAnswer {
export interface EditQuestionAnswerIn {
    id: number;
    questionAnswerCategoryId: number;
    name: string;
    description: string;
    icon: string;
    isActive: boolean;
    isPublish: boolean;
    questionAnswerAttachements: EQuestionAnswerAttachementIn[];
}

export interface EQuestionAnswerAttachementIn {
    questionAnswerId: number | null;
    attachment: string | null;
    attachmentType: string | null;
}

}
export namespace QuestionAnswer.EditQuestionAnswer {
export interface EditQuestionAnswerOut extends BaseResponseOut {
    editedQuestionAnswerOut: EditedQuestionAnswerOut;
}

export interface EditedQuestionAnswerOut {
    id: number;
}

}
export namespace QuestionAnswer.GetQuestionAnswerInfoById {
export interface GetQuestionAnswerByIdIn {
    id: number;
}

}
export namespace QuestionAnswer.GetQuestionAnswerInfoById {
export interface GetQuestionAnswerByIdOut extends BaseResponseOut {
    questionAnswerByIdOut: QuestionAnswerByIdOut;
}

export interface QuestionAnswerByIdOut {
    id: number;
    questionAnswerCategoryId: number | null;
    name: string | null;
    description: string | null;
    icon: string | null;
    isActive: boolean | null;
    isPublish: boolean | null;
    questionAnswerAttachements: QuestionAnswerAttachements[];
}

export interface QuestionAnswerAttachements {
    questionAnswerAttachementId: number;
    attachment: string | null;
    attachmentType: string | null;
}

}
export namespace QuestionAnswer.GetQuestionAnswers {
export interface GetQuestionAnswersIn {
    searchValue: string | null;
    pageNo: number;
    pageSize: number;
    sortColumn: string | null;
    sortOrder: string | null;
    categoryId: number | null;
    isViewAll: boolean | null;
    isShowActiveOnly: boolean | null;
}

}
export namespace QuestionAnswer.GetQuestionAnswers {
export interface GetQuestionAnswersOut extends BaseResponseOut {
    customersQuestionAnswersOut: QuestionAnswersOut[];
}

export interface QuestionAnswersOut {
    id: number;
    questionAnswerCategoryId: number | null;
    name: string | null;
    description: string | null;
    icon: string | null;
    isActive: boolean | null;
    isPublish: boolean | null;
    filteredCount: number | null;
    questionAnswerAttachementId: number;
    attachment: string | null;
    attachmentType: string | null;
    status: string | null;
}

}
export namespace QuestionAnswer.UpdateQuestionAnswer {
export interface UpdateQuestionAnswerIn {
    id: number;
    isActive: boolean;
}

}
export namespace QuestionAnswer.UpdateQuestionAnswer {
export interface UpdateQuestionAnswerStatusOut extends BaseResponseOut {

}

}
export namespace QuestionAnswer.UpdateQuestionAnswerStatus {
export interface UpdateQuestionAnswerStatusIn {
    id: number;
    isPublish: boolean;
}

}
export namespace QuestionAnswer.UpdateQuestionAnswerStatus {
export interface UpdateQuestionAnswerStatusOut extends BaseResponseOut {

}

}
export namespace rCustomerCommunication.GetCustomerCommunication {
export interface GetCustomerCommunicationIn {
    customerId: number;
    searchString: string;
    pageNo: number | null;
    pageSize: number | null;
    userLevel: string | null;
    customerUserId: number | null;
}

}
export namespace rCustomerCommunication.GetCustomerCommunication {
export interface GetCustomerCommunicationOut extends BaseResponseOut {
    customerCommunicationOut: CustomerCommunicationOut[];
}

export interface CustomerCommunicationOut {
    id: number | null;
    customerId: number | null;
    customerReservationId: number | null;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    picture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    country: string | null;
    language: string | null;
    userType: string | null;
    chatType: string | null;
}

}
export namespace rCustomerCommunication.GetCustomerCommunicationByReservationId {
export interface GetCustomerCommunicationByReservationIdIn {
    id: number;
    reservationId: number | null;
}

}
export namespace rCustomerCommunication.GetCustomerCommunicationByReservationId {
export interface GetCustomerCommunicationByReservationOut extends BaseResponseOut {
    getCustomerCommunicationByReservationIdResponseOut: GetCustomerCommunicationByReservationIdResponseOut;
}

export interface GetCustomerCommunicationByReservationIdResponseOut {
    id: number | null;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    profilePicture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    country: string | null;
    language: string | null;
    roomNumber: string | null;
    createdAt: string | null;
    checkinDate: string | null;
    checkoutDate: string | null;
}

}
export namespace ReplicateDataForGuestApp.ReplicateGuestData {
export interface ReplicateDataIn {
    appBuilderId: number;
    newBuilderId: number;
}

}
export namespace ReplicateDataForGuestApp.ReplicateGuestData {
export interface ReplicateDataOut extends BaseResponseOut {

}

export interface CustomerGuestAppBuildersOutId {
    id: number | null;
    customerId: number | null;
    customerRoomNameId: number | null;
    message: string | null;
    secondaryMessage: string | null;
    localExperience: boolean | null;
    ekey: boolean | null;
    propertyInfo: boolean | null;
    enhanceYourStay: boolean | null;
    reception: boolean | null;
    housekeeping: boolean | null;
    roomService: boolean | null;
    concierge: boolean | null;
    transferServices: boolean | null;
    onlineCheckIn: boolean | null;
    isActive: boolean | null;
    isWork: number | null;
    displayOrderForGuestBuilder: ScreenDisplayOrderAndStatusBuilderOut[] | null;
    customerPropertyinfo: PropertyInfoOut[] | null;
    customerGuestAppEnhanceYourStayCategories: CustomerGuestAppEnhanceYourStayCategoryOut[];
    customerGuestAppReceptionCategories: CustomerGuestAppReceptionCategoryOut[];
    customerGuestAppHousekeepingCategories: CustomerGuestAppHousekeepingCategoryOut[];
    customerGuestAppRoomServiceCategories: CustomerGuestAppRoomServiceCategoryOut[];
    customerGuestAppConciergeCategories: CustomerGuestAppConciergeCategoryOut[];
}

export interface ScreenDisplayOrderAndStatusBuilderOut {
    id: number | null;
    screenName: number | null;
    jsonData: string | null;
    refrenceId: number | null;
    isActive: boolean | null;
}

export interface PropertyInfoOut {
    id: number | null;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    wifiUsername: string | null;
    wifiPassword: string | null;
    overview: string | null;
    checkInPolicy: string | null;
    termsAndConditions: string | null;
    street: string | null;
    streetNumber: string | null;
    city: string | null;
    postalcode: string | null;
    isActive: boolean | null;
    country: string | null;
    latitude: string | null;
    longitude: string | null;
    isPublish: boolean | null;
    customerPropertyServices: CustomerPropertyServiceOut[] | null;
    customerPropertyGallery: CustomerPropertyGalleryOut[] | null;
    customerPropertyExtras: CustomerPropertyExtraOut[] | null;
    customerPropertyEmergencyNo: CustomerPropertyEmergencyNumberOut[] | null;
    displayOrderForPropertyInfo: ScreenDisplayOrderAndStatusOut[] | null;
}

export interface ScreenDisplayOrderAndStatusOut {
    id: number | null;
    screenName: number | null;
    jsonData: string | null;
    refrenceId: number | null;
    isActive: boolean | null;
}

export interface CustomerPropertyServiceOut {
    id: number | null;
    customerPropertyInformationId: number | null;
    name: string | null;
    isActive: boolean | null;
    icon: string | null;
    description: string | null;
    isPublish: boolean | null;
    customerPropertyServiceImages: CustomerPropertyServiceImageOut[] | null;
}

export interface CustomerPropertyServiceImageOut {
    id: number | null;
    customerPropertyServiceId: number | null;
    serviceImages: string | null;
    isPublish: boolean | null;
    isActive: boolean | null;
}

export interface CustomerPropertyGalleryOut {
    id: number | null;
    customerPropertyInformationId: number | null;
    propertyImage: string | null;
    isPublish: boolean | null;
    isActive: boolean | null;
}

export interface CustomerPropertyExtraOut {
    id: number | null;
    customerPropertyInformationId: number | null;
    extraType: number | null;
    name: string | null;
    isPublish: boolean | null;
    isActive: boolean | null;
    displayOrder: number | null;
    customerPropertyExtraDetails: CustomerPropertyExtraDetailsOut[];
}

export interface CustomerPropertyExtraDetailsOut {
    id: number | null;
    customerPropertyExtraId: number | null;
    description: string | null;
    latitude: string | null;
    longitude: string | null;
    isPublish: boolean | null;
    isActive: boolean | null;
}

export interface CustomerPropertyEmergencyNumberOut {
    id: number | null;
    customerPropertyInformationId: number | null;
    name: string | null;
    phoneNumber: string | null;
    isPublish: boolean | null;
    isActive: boolean | null;
    displayOrder: number | null;
}

export interface CustomerGuestAppEnhanceYourStayItemOut {
    id: number | null;
    customerGuestAppBuilderId: number | null;
    customerId: number | null;
    customerGuestAppBuilderCategoryId: number | null;
    badge: number | null;
    shortDescription: string | null;
    longDescription: string | null;
    buttonType: number | null;
    buttonText: string | null;
    chargeType: number | null;
    discount: number | null;
    price: number | null;
    isActive: boolean | null;
    currency: string | null;
    displayOrder: number | null;
    isPublish: boolean | null;
    customerGuestAppEnhanceYourStayItemsImages: CustomerGuestAppEnhanceYourStayItemsImageOut[] | null;
    customerGuestAppEnhanceYourStayCategoryItemsExtras: CustomerGuestAppEnhanceYourStayCategoryItemsExtraOut[] | null;
}

export interface CustomerGuestAppEnhanceYourStayCategoryOut {
    id: number | null;
    customerGuestAppBuilderId: number | null;
    customerId: number | null;
    isActive: boolean | null;
    categoryName: string | null;
    displayOrder: number | null;
    isPublish: boolean | null;
    customerGuestAppEnhanceYourStayItems: CustomerGuestAppEnhanceYourStayItemOut[] | null;
}

export interface CustomerGuestAppEnhanceYourStayItemsImageOut {
    id: number | null;
    customerGuestAppEnhanceYourStayItemId: number | null;
    isActive: boolean | null;
    itemsImages: string | null;
    disaplayOrder: number | null;
    isPublish: boolean | null;
}

export interface CustomerGuestAppEnhanceYourStayCategoryItemsExtraOut {
    id: number | null;
    customerGuestAppEnhanceYourStayItemId: number | null;
    queType: number | null;
    questions: string | null;
    optionValues: string | null;
    isPublish: boolean | null;
    isActive: boolean | null;
}

export interface CustomerGuestAppReceptionItemOut {
    id: number | null;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    customerGuestAppReceptionCategoryId: number | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    isPublish: boolean | null;
    isActive: boolean | null;
}

export interface CustomerGuestAppReceptionCategoryOut {
    id: number | null;
    customerGuestAppBuilderId: number | null;
    customerId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    isPublish: boolean | null;
    isActive: boolean | null;
    receptionItem: CustomerGuestAppReceptionItemOut[] | null;
}

export interface CustomerGuestAppHousekeepingItemOut {
    id: number | null;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    customerGuestAppHousekeepingCategoryId: number | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    isPublish: boolean | null;
    isActive: boolean | null;
}

export interface CustomerGuestAppHousekeepingCategoryOut {
    id: number | null;
    customerGuestAppBuilderId: number | null;
    customerId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    isPublish: boolean | null;
    isActive: boolean | null;
    houseItem: CustomerGuestAppHousekeepingItemOut[] | null;
}

export interface CustomerGuestAppRoomServiceItemOut {
    id: number | null;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    customerGuestAppRoomServiceCategoryId: number | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    isPublish: boolean | null;
    isActive: boolean | null;
}

export interface CustomerGuestAppRoomServiceCategoryOut {
    id: number | null;
    customerGuestAppBuilderId: number | null;
    customerId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    isPublish: boolean | null;
    isActive: boolean | null;
    roomItem: CustomerGuestAppRoomServiceItemOut[] | null;
}

export interface CustomerGuestAppConciergeItemOut {
    id: number | null;
    customerId: number | null;
    customerGuestAppBuilderId: number | null;
    customerGuestAppConciergeCategoryId: number | null;
    name: string | null;
    itemsMonth: boolean | null;
    itemsDay: boolean | null;
    itemsMinute: boolean | null;
    itemsHour: boolean | null;
    quantityBar: boolean | null;
    itemLocation: boolean | null;
    comment: boolean | null;
    isPriceEnable: boolean | null;
    price: number | null;
    currency: string | null;
    displayOrder: number | null;
    isPublish: boolean | null;
    isActive: boolean | null;
}

export interface CustomerGuestAppConciergeCategoryOut {
    id: number | null;
    customerGuestAppBuilderId: number | null;
    customerId: number | null;
    categoryName: string | null;
    displayOrder: number | null;
    isPublish: boolean | null;
    isActive: boolean | null;
    conciergeitem: CustomerGuestAppConciergeItemOut[] | null;
}

}
export namespace TaxiTransfer.CancelGuestTaxiTransferRequest {
export interface CancelGuestTaxiTransferRequestIn {
    id: number;
    customerId: number;
    guestId: number;
    transferId: string | null;
    transferStatus: string | null;
    refundAmount: number | null;
    paymentId: string | null;
    paymentDetails: string | null;
    transferJson: string | null;
    refundId: string | null;
    refundStatus: string | null;
}

}
export namespace TaxiTransfer.CancelGuestTaxiTransferRequest {
export interface CancelGuestTaxiTransferRequestOut extends BaseResponseOut {

}

}
export namespace TaxiTransfer.CreateGuestTaxiTransferRequest {
export interface CreateGuestTaxiTransferRequestIn {
    customerId: number;
    guestId: number;
    transferId: string | null;
    transferStatus: string | null;
    paymentId: string | null;
    paymentDetails: string | null;
    transferJson: string | null;
    fareAmount: number | null;
    hospitioFareAmount: number | null;
    pickUpDate: string | null;
    extraDetails: ExtraDetails | null;
}

export interface ExtraDetails {
    luggage: string | null;
    passenger: string | null;
    fromLocation: string | null;
    toLocation: string | null;
    refundDate: string | null;
}

}
export namespace TaxiTransfer.CreateGuestTaxiTransferRequest {
export interface CreateGuestTaxiTransferRequestOut extends BaseResponseOut {

}

}
export namespace TaxiTransfer.DeleteTaxiTransferMonthlyReport {
export interface DeleteTaxiTransferMonthlyReportIn {
    month: string | null;
    year: string | null;
}

}
export namespace TaxiTransfer.DeleteTaxiTransferMonthlyReport {
export interface DeleteTaxiTransferMonthlyReportOut extends BaseResponseOut {

}

}
export namespace TaxiTransfer.GetAllTransferData {
export interface GetAllTransferDataIn {
    searchValue: string | null;
    pageNo: number | null;
    pageSize: number | null;
    customerId: number | null;
    guestId: number | null;
    fromCreateAt: string | null;
    toCreateAt: string | null;
}

}
export namespace TaxiTransfer.GetAllTransferData {
export interface GetAllTransferDataOut extends BaseResponseOut {
    taxiTransferResponse: TaxiTransferResponse[];
}

export interface TaxiTransferResponse {
    id: number;
    customerId: number;
    customerName: string | null;
    guestId: number | null;
    guestFirstName: string | null;
    guestLastName: string | null;
    reservationId: string | null;
    transferStatus: string | null;
    transferId: string | null;
    gRPaymentId: string | null;
    hospitioFareAmount: number | null;
    fareAmount: number | null;
    markUpAmount: number | null;
    refundStatus: string | null;
    refundAmount: number | null;
    refundId: string | null;
    luggage: string | null;
    passenger: string | null;
    fromLocation: string | null;
    toLocation: string | null;
    formLocationDescription: string | null;
    toLocationDescription: string | null;
    pickUpDateTime: string | null;
    bookedDateTime: string | null;
    cancelledAt: string | null;
    passengerCount: string | null;
    passengerName: string | null;
    passengerEmail: string | null;
    passengerMobile: string | null;
    currency: string | null;
    gRCreateAt: string | null;
    paymentServiceTransactionId: string | null;
    paymentMethod: string | null;
    paymentServiceRefeunfId: string | null;
    filterCount: number | null;
}

}
export namespace TaxiTransfer.GetDownloadTransferData {
export interface GetDownloadTransferDataIn {
    month: string | null;
    year: string | null;
}

}
export namespace TaxiTransfer.GetDownloadTransferData {
export interface GetDownloadTransferDataOut extends BaseResponseOut {
    downloadTransferData: string;
}

export interface DownloadTaxiTransferResponse {
    customerName: string | null;
    guestFirstName: string | null;
    guestLastName: string | null;
    transferId: string | null;
    pickUpDateTime: string | null;
    fareAmount: number | null;
    markUpAmount: number | null;
    hospitioFareAmount: number | null;
}

}
export namespace TaxiTransfer.GetTaxiTransferMonths {
export interface GetTaxiTransferMonthsIn {
    year: string | null;
}

}
export namespace TaxiTransfer.GetTaxiTransferMonths {
export interface GetTaxiTransferMonthsOut extends BaseResponseOut {
    taxiTransferMonths: TaxiTransferMonths;
}

export interface TaxiTransferMonths {
    monthNumber: number[] | null;
}

}
export namespace TaxiTransfer.GetTaxiTransferYears {
export interface GetTaxiTransferYearsIn {

}

}
export namespace TaxiTransfer.GetTaxiTransferYears {
export interface GetTaxiTransferYearsOut extends BaseResponseOut {
    taxiTransferYears: TaxiTransferYears;
}

export interface TaxiTransferYears {
    years: number[] | null;
}

}
export namespace TaxiTransfer.GetTransferDataByGuestId {
export interface GetTransferDataByGuestIdIn {
    guestId: string | null;
    customerId: string | null;
}

}
export namespace TaxiTransfer.GetTransferDataByGuestId {
export interface GetTransferDataByGuestIdOut extends BaseResponseOut {
    transferResponse: TransferResponse[];
}

export interface TransferResponse {
    id: number;
    guestId: number | null;
    transferId: string | null;
    transferStatus: string | null;
    refundAmount: number | null;
    gRPaymentId: string | null;
    gRPaymentDetails: string | null;
    transferJson: string | null;
    refundId: string | null;
    refundStatus: string | null;
    fareAmount: number | null;
    hospitioFareAmount: number | null;
    transferModel: TransferModel | null;
    extraDetailsJson: string | null;
    extraDetails: ExtraDetails | null;
}

export interface TransferModel {
    data: Data | null;
}

export interface Data {
    id: string | null;
    type: string | null;
    attributes: Attributes | null;
}

export interface Attributes {
    quote_id: string | null;
    passenger_booking_reference: string | null;
    transfer_status: string | null;
    traveler_no_show_up: string | null;
    cancellation_reason: string | null;
    traveler_cancellation_reason: string | null;
    cancelled_at: string | null;
    pickup_date_time: string | null;
    booked_date_time: string | null;
    from_location: Location | null;
    to_location: Location | null;
    passenger: Passenger | null;
    agent: Agent | null;
    service_info: ServiceInfo | null;
    fare: Fare | null;
    driver: string | null;
    additional_notes: string | null;
    transfer_events: transferevents[] | null;
}

export interface Location {
    type: string | null;
    description: string | null;
    lat: number | null;
    lng: number | null;
}

export interface Passenger {
    count: number | null;
    name: string | null;
    email: string | null;
    mobile: string | null;
}

export interface Agent {
    name: string | null;
    email: string | null;
    phone: string | null;
}

export interface ServiceInfo {
    type: string | null;
    vehicle_type: string | null;
    max_pax: number | null;
    max_lug: number | null;
    photo_url: string | null;
    photo_urls: string[] | null;
    description: string | null;
    supplier: Supplier | null;
    passenger_reviews: PassengerReviews | null;
}

export interface Supplier {
    id: string | null;
    name: string | null;
    photo_url: string | null;
    description: string | null;
}

export interface PassengerReviews {
    count: number | null;
    average_rating: number | null;
}

export interface Fare {
    price: number | null;
    currency_code: string | null;
    type: string | null;
    refund_cancellation_policy: string | null;
    refund_policies: Policy[] | null;
    refund: Refund | null;
}

export interface Refund {
    policy: Policy | null;
    amount: number | null;
}

export interface Policy {
    hours_from_operation: hoursfromoperation | null;
    refund_percentage: string | null;
}

export interface hoursfromoperation {
    min: string | null;
    max: string | null;
}

export interface transferevents {
    name: string | null;
    timestamp: string | null;
}

export interface ExtraDetails {
    luggage: string | null;
    passenger: string | null;
    fromLocation: string | null;
    toLocation: string | null;
    refundDate: string | null;
}

}
export namespace Ticket.CloseTicket {
export interface CloseTicketIn {
    ticketId: number;
}

}
export namespace Ticket.CloseTicket {
export interface CloseTicketOut extends BaseResponseOut {

}

}
export namespace Ticket.CreateTicket {
export interface CreateTicketIn {
    customerId: number | null;
    title: string | null;
    details: string | null;
    priority: number | null;
    duedate: string | null;
    cSAgentId: number | null;
    status: number | null;
}

}
export namespace Ticket.CreateTicket {
export interface CreateTicketOut extends BaseResponseOut {

}

}
export namespace Ticket.CreateTicketReply {
export interface CreateTicketReplyIn {
    ticketId: number | null;
    reply: string | null;
}

}
export namespace Ticket.CreateTicketReply {
export interface CreateTicketReplyOut extends BaseResponseOut {

}

}
export namespace Ticket.ForwardTicket {
export interface ForwardTicketIn {
    id: number;
    userId: number;
    groupId: number;
}

}
export namespace Ticket.ForwardTicket {
export interface ForwardTicketOut extends BaseResponseOut {

}

export interface ForwardTicket {

}

}
export namespace Ticket.GetRecentTickets {
export interface GetRecentTicketIn {
    customerId: number;
}

}
export namespace Ticket.GetRecentTickets {
export interface GetRecentTicketOut extends BaseResponseOut {
    getTicketsResponseOut: GetRecentTicketsResponseOut[];
}

export interface GetRecentTicketsResponseOut {
    id: number;
    customerId: number | null;
    title: string | null;
    details: string | null;
    priority: number | null;
    duedate: string | null;
    ticketCategoryId: number | null;
    cSAgentId: number | null;
    status: number | null;
    closeDate: string | null;
    createdFrom: number | null;
    createdAt: string | null;
}

}
export namespace Ticket.GetTicketById {
export interface GetTicketByIdIn {

}

}
export namespace Ticket.GetTicketById {
export interface GetTicketByIdOut extends BaseResponseOut {
    getTicketByIdResponseOut: GetTicketByIdResponseOut[];
}

export interface GetTicketByIdResponseOut {
    id: number;
    customerId: number | null;
    customerName: string | null;
    title: string | null;
    details: string | null;
    priority: number | null;
    duedate: string | null;
    cSAgentId: number | null;
    cSAgentName: string | null;
    status: number | null;
    closeDate: string | null;
    createdFrom: number | null;
    createdAt: string;
    replies: GetTicketByIdRepliesResponseOut[] | null;
}

export interface GetTicketByIdRepliesResponseOut {
    id: number;
    ticketId: number | null;
    reply: string | null;
    userName: string | null;
    createdAt: string;
    createdBy: number | null;
    createdFrom: number | null;
}

}
export namespace Ticket.GetTickets {
export interface GetTicketsIn {

}

}
export namespace Ticket.GetTickets {
export interface GetTicketsOut extends BaseResponseOut {
    getTicketsResponseOut: GetTicketsResponseOut[];
}

export interface GetTicketsResponseOut {
    id: number;
    customerId: number | null;
    customerName: string | null;
    title: string | null;
    details: string | null;
    priority: number | null;
    duedate: string | null;
    ticketCategoryId: number | null;
    ticketCategoryName: string | null;
    cSAgentId: number | null;
    cSAgentName: string | null;
    status: number | null;
    closeDate: string | null;
    createdFrom: number | null;
}

}
export namespace Ticket.GetTicketsWithFilters {
export interface GetTicketsWithFiltersIn {
    pageNo: number;
    pageSize: number;
    categoryId: number | null;
    status: number | null;
    priority: number | null;
    customerId: number | null;
    cSAgentId: number | null;
    fromCreate: string | null;
    toCreate: string | null;
    fromClose: string | null;
    toClose: string | null;
    shortBy: number;
    createdFrom: number;
    applyPagination: boolean;
}

}
export namespace Ticket.GetTicketsWithFilters {
export interface GetTicketsWithFiltersOut extends BaseResponseOut {
    getTicketsWithFiltersResponseOut: GetTicketsWithFiltersResponseOut[];
}

export interface GetTicketsWithFiltersResponseOut {
    id: number;
    customerId: number | null;
    businessName: string | null;
    customerName: string | null;
    email: string | null;
    title: string | null;
    details: string | null;
    priority: number | null;
    duedate: string | null;
    ticketCategoryName: string | null;
    cSAgentName: string | null;
    status: number | null;
    closeDate: string | null;
    createdFrom: number | null;
    isActive: boolean | null;
    createdAt: string | null;
    maxCreatedDate: string | null;
    profilePicture: string | null;
    filteredCount: number;
    totalUnReadCount: number;
}

}
export namespace Ticket.UpdateTicketPriority {
export interface UpdateTicketPriorityIn {
    id: number;
    priority: number | null;
}

}
export namespace Ticket.UpdateTicketPriority {
export interface UpdateTicketPriorityOut extends BaseResponseOut {
    updatedTicketStatusOut: UpdatedTicketPriorityOut;
}

export interface UpdatedTicketPriorityOut {
    id: number;
    priority: number | null;
}

}
export namespace TicketCategories.CreateTicketCategory {
export interface CreateTicketCategoryIn {
    name: string;
    isActive: boolean;
    createdAt: string | null;
    createdBy: number;
}

}
export namespace TicketCategories.CreateTicketCategory {
export interface CreateTicketCategoryOut extends BaseResponseOut {
    createdTicketCategoryOut: CreatedTicketCategoryOut;
}

export interface CreatedTicketCategoryOut {
    id: number;
    name: string;
    isActive: boolean;
}

}
export namespace TicketCategories.DeleteTicketCategory {
export interface DeleteTicketCategoryIn {
    id: number;
    deletedAt: string | null;
}

}
export namespace TicketCategories.DeleteTicketCategory {
export interface DeleteTicketCategoryOut extends BaseResponseOut {
    deletedtTicketCategoryOut: DeletedtTicketCategoryOut;
}

export interface DeletedtTicketCategoryOut {
    id: number;
}

}
export namespace TicketCategories.GetTicketCategories {
export interface GetTicketCategoriesIn {

}

}
export namespace TicketCategories.GetTicketCategories {
export interface GetTicketCategoriesOut extends BaseResponseOut {
    ticketCategoriesOuts: TicketCategoriesOut[];
}

export interface TicketCategoriesOut {
    id: number;
    name: string;
}

}
export namespace TicketCategories.GetTicketCategory {
export interface GetTicketCategoryIn {
    id: number;
}

}
export namespace TicketCategories.GetTicketCategory {
export interface GetTicketCategoryOut extends BaseResponseOut {
    ticketCategoryOut: TicketCategoryOut;
}

export interface TicketCategoryOut {
    id: number;
    name: string;
}

}
export namespace TicketCategories.UpdateTicketCategory {
export interface UpdateTicketCategoryIn {
    id: number;
    categoryName: string;
    isActive: boolean;
    updateAt: string | null;
}

}
export namespace TicketCategories.UpdateTicketCategory {
export interface UpdateTicketCategoryOut extends BaseResponseOut {
    updatedTicketCategoryOut: UpdatedTicketCategoryOut;
}

export interface UpdatedTicketCategoryOut {
    id: number;
    categoryName: string;
    isActive: boolean;
}

}
export namespace Transactions.CaptureAdminTransaction {
export interface CaptureAdminTransactionIn {
    customerId: number | null;
    transaction_Id: string | null;
    amount: number | null;
}

}
export namespace Transactions.CaptureAdminTransaction {
export interface CaptureAdminTransactionOut extends BaseResponseOut {
    capturedAdminTransactionOut: CapturedAdminTransactionOut;
}

export interface CapturedAdminTransactionOut {
    type: string | null;
    id: string | null;
    merchant_account_id: string | null;
    status: string | null;
    intent: string | null;
    amount: number | null;
    captured_amount: number | null;
    refunded_amount: number | null;
    currency: string | null;
    country: string | null;
    payment_method: PaymentMethod | null;
    buyer: Buyer | null;
    created_at: string | null;
    external_identifier: string | null;
    updated_at: string | null;
    payment_service: PaymentService | null;
    pending_review: boolean | null;
    merchant_initiated: boolean | null;
    payment_source: string | null;
    is_subsequent_payment: boolean | null;
    statement_descriptor: StatementDescriptor | null;
    cart_items: CartItem[] | null;
    scheme_transaction_id: string | null;
    raw_response_code: string | null;
    raw_response_description: string | null;
    avs_response_code: string | null;
    cvv_response_code: string | null;
    method: string | null;
    payment_service_transaction_id: string | null;
    metadata: { [key: string]: string; } | null;
    shipping_details: ShippingDetails | null;
    three_d_secure: ThreeDSecure | null;
    authorized_at: string | null;
    captured_at: string | null;
    voided_at: string | null;
    checkout_session_id: string | null;
}

export interface PaymentMethod {
    type: string | null;
    id: string | null;
    method: string | null;
    external_identifier: string | null;
    label: string | null;
    scheme: string | null;
    expiration_date: string | null;
    approval_target: string | null;
    approval_url: string | null;
    currency: string | null;
    country: string | null;
    details: Details | null;
}

export interface Buyer {
    type: string | null;
    id: string | null;
    external_identifier: string | null;
    display_name: string | null;
    billing_details: BillingDetails | null;
}

export interface BillingDetails {
    type: string | null;
    first_name: string | null;
    last_name: string | null;
    email_address: string | null;
    phone_number: string | null;
    address: Address | null;
    tax_id: TaxId | null;
}

export interface PaymentService {
    type: string | null;
    id: string | null;
    payment_service_definition_id: string | null;
    method: string | null;
    display_name: string | null;
}

export interface StatementDescriptor {
    name: string | null;
    description: string | null;
    city: string | null;
    phone_number: string | null;
    url: string | null;
}

export interface CartItem {
    name: string | null;
    quantity: number | null;
    unit_amount: number | null;
    discount_amount: any | null;
    tax_amount: any | null;
    external_identifier: string | null;
    sku: string | null;
    product_url: string | null;
    image_url: string | null;
    categories: string[] | null;
    product_type: string | null;
}

export interface ShippingDetails {
    type: string | null;
    id: string | null;
    buyer_id: string | null;
    first_name: string | null;
    last_name: string | null;
    email_address: string | null;
    phone_number: string | null;
    address: Address | null;
}

export interface ThreeDSecure {
    version: string | null;
    status: string | null;
    method: string | null;
    error_data: ErrorData | null;
    response_data: any | null;
}

export interface Details {
    card_type: string | null;
    bin: string | null;
}

export interface Address {
    city: string | null;
    country: string | null;
    postal_code: string | null;
    state: string | null;
    state_code: string | null;
    house_number_or_name: string | null;
    line1: string | null;
    line2: string | null;
    organization: string | null;
}

export interface TaxId {
    value: string | null;
    kind: string | null;
}

export interface ErrorData {
    description: string | null;
    detail: string | null;
    code: string | null;
    component: string | null;
}

}
export namespace Transactions.CaptureTransaction {
export interface CaptureTransactionIn {
    customerId: number;
    transaction_Id: string | null;
    amount: number | null;
}

}
export namespace Transactions.CaptureTransaction {
export interface CaptureTransactionOut extends BaseResponseOut {
    capturedTransactionOut: CapturedTransactionOut;
}

export interface CapturedTransactionOut {
    type: string | null;
    id: string | null;
    merchant_account_id: string | null;
    status: string | null;
    intent: string | null;
    amount: number | null;
    captured_amount: number | null;
    refunded_amount: number | null;
    currency: string | null;
    country: string | null;
    payment_method: PaymentMethod | null;
    buyer: Buyer | null;
    created_at: string | null;
    external_identifier: string | null;
    updated_at: string | null;
    payment_service: PaymentService | null;
    pending_review: boolean | null;
    merchant_initiated: boolean | null;
    payment_source: string | null;
    is_subsequent_payment: boolean | null;
    statement_descriptor: StatementDescriptor | null;
    cart_items: CartItem[] | null;
    scheme_transaction_id: string | null;
    raw_response_code: string | null;
    raw_response_description: string | null;
    avs_response_code: string | null;
    cvv_response_code: string | null;
    method: string | null;
    payment_service_transaction_id: string | null;
    metadata: { [key: string]: string; } | null;
    shipping_details: ShippingDetails | null;
    three_d_secure: ThreeDSecure | null;
    authorized_at: string | null;
    captured_at: string | null;
    voided_at: string | null;
    checkout_session_id: string | null;
}

export interface PaymentMethod {
    type: string | null;
    id: string | null;
    method: string | null;
    external_identifier: string | null;
    label: string | null;
    scheme: string | null;
    expiration_date: string | null;
    approval_target: string | null;
    approval_url: string | null;
    currency: string | null;
    country: string | null;
    details: Details | null;
}

export interface Buyer {
    type: string | null;
    id: string | null;
    external_identifier: string | null;
    display_name: string | null;
    billing_details: BillingDetails | null;
}

export interface BillingDetails {
    type: string | null;
    first_name: string | null;
    last_name: string | null;
    email_address: string | null;
    phone_number: string | null;
    address: Address | null;
    tax_id: TaxId | null;
}

export interface PaymentService {
    type: string | null;
    id: string | null;
    payment_service_definition_id: string | null;
    method: string | null;
    display_name: string | null;
}

export interface StatementDescriptor {
    name: string | null;
    description: string | null;
    city: string | null;
    phone_number: string | null;
    url: string | null;
}

export interface CartItem {
    name: string | null;
    quantity: number | null;
    unit_amount: number | null;
    discount_amount: any | null;
    tax_amount: any | null;
    external_identifier: string | null;
    sku: string | null;
    product_url: string | null;
    image_url: string | null;
    categories: string[] | null;
    product_type: string | null;
}

export interface ShippingDetails {
    type: string | null;
    id: string | null;
    buyer_id: string | null;
    first_name: string | null;
    last_name: string | null;
    email_address: string | null;
    phone_number: string | null;
    address: Address | null;
}

export interface ThreeDSecure {
    version: string | null;
    status: string | null;
    method: string | null;
    error_data: ErrorData | null;
    response_data: any | null;
}

export interface Details {
    card_type: string | null;
    bin: string | null;
}

export interface Address {
    city: string | null;
    country: string | null;
    postal_code: string | null;
    state: string | null;
    state_code: string | null;
    house_number_or_name: string | null;
    line1: string | null;
    line2: string | null;
    organization: string | null;
}

export interface TaxId {
    value: string | null;
    kind: string | null;
}

export interface ErrorData {
    description: string | null;
    detail: string | null;
    code: string | null;
    component: string | null;
}

}
export namespace UserAccount.CreateUserAccount {
export interface CreateUserAccountIn {
    id: number | null;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    title: string | null;
    profilePicture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    departmentId: number | null;
    groupId: number | null;
    userLevelId: number | null;
    supervisorId: number | null;
    userName: string | null;
    password: string | null;
    isActive: boolean;
    userModulePermissions: CreateUsersPermissionIn[];
}

export interface CreateUsersPermissionIn {
    id: number | null;
    permissionId: number | null;
    userId: number | null;
    isView: boolean | null;
    isEdit: boolean | null;
    isUpload: boolean | null;
    isReply: boolean | null;
    isSend: boolean | null;
}

}
export namespace UserAccount.CreateUserAccount {
export interface CreateUserAccountOut extends BaseResponseOut {

}

}
export namespace UserAccount.DeleteUserAccount {
export interface DeleteUserAccountIn {
    userId: number;
}

}
export namespace UserAccount.DeleteUserAccount {
export interface DeleteUserAccountOut extends BaseResponseOut {
    deleteUser: DeleteUser;
}

export interface DeleteUser {
    deleteUserId: number;
}

}
export namespace UserAccount.EditUserAccount {
export interface EditUserAccountIn {
    id: number | null;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    title: string | null;
    profilePicture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    departmentId: number | null;
    groupId: number | null;
    userLevelId: number | null;
    supervisorId: number | null;
    userName: string | null;
    password: string | null;
    isActive: boolean;
    userModulePermissions: EditUsersPermissionIn[];
}

export interface EditUsersPermissionIn {
    id: number | null;
    permissionId: number | null;
    userId: number | null;
    isView: boolean | null;
    isEdit: boolean | null;
    isUpload: boolean | null;
    isReply: boolean | null;
    isSend: boolean | null;
}

}
export namespace UserAccount.EditUserAccount {
export interface EditUserAccountOut extends BaseResponseOut {

}

}
export namespace UserAccount.GetDepartmentsUsers {
export interface GetDepartmentsUsersIn extends BaseSearchFilterOptions {

}

}
export namespace UserAccount.GetDepartmentsUsers {
export interface GetDepartmentsUsersOut extends BaseResponseOut {
    deptWiseUserOut: DepartmentsOut[];
}

export interface DepartmentsOut {
    id: number | null;
    name: string | null;
    ceoId: number | null;
    ceoName: string | null;
    managerId: number | null;
    managerName: string | null;
    filteredCount: number | null;
    isActive: boolean;
    groups: GroupOut[] | null;
}

export interface GroupOut {
    id: number | null;
    name: string | null;
    groupLeaderId: number | null;
    groupLeader: string | null;
    isActive: boolean;
    usersOut: UserOut[] | null;
}

export interface UserOut {
    id: number | null;
    firstName: string | null;
    lastName: string | null;
    isActive: boolean;
}

}
export namespace UserAccount.GetUserById {
export interface GetUserByIdIn extends BaseSearchFilterOptions {
    id: number;
}

}
export namespace UserAccount.GetUserById {
export interface GetUserByIdOut extends BaseResponseOut {
    userByIdOut: UserByIdOut;
}

export interface UserByIdOut {
    id: number | null;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    title: string | null;
    profilePicture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    departmentId: number | null;
    groupId: number | null;
    userLevelId: number | null;
    supervisorId: number | null;
    userName: string | null;
    password: string | null;
    isActive: boolean;
    userModulePermissions: UserPermissionsOut[];
}

export interface UserPermissionsOut {
    id: number;
    permissionId: number | null;
    userId: number | null;
    isView: boolean | null;
    isEdit: boolean | null;
    isUpload: boolean | null;
    isReply: boolean | null;
    isSend: boolean | null;
}

}
export namespace UserAccount.GetUserProfile {
export interface GetUserProfileIn {
    userId: number;
    userType: string;
}

}
export namespace UserAccount.GetUserProfile {
export interface GetUserProfileOut extends BaseResponseOut {
    getProfileOut: GetProfileOut;
}

export interface GetProfileOut {
    id: number | null;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    title: string | null;
    profilePicture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    departmentName: string | null;
    incomingTranslationLangage: string | null;
    noTranslateWords: string | null;
    userName: string | null;
    isActive: boolean | null;
    password: string | null;
    groupName: string | null;
    supervisorName: string | null;
    levelName: string | null;
    userUniqueId: string | null;
    taxiTransferCommission: number | null;
}

}
export namespace UserAccount.GetUsers {
export interface GetUsersIn {
    departmentId: number | null;
    groupId: number | null;
    userLevelId: number | null;
    userId: number | null;
}

}
export namespace UserAccount.GetUsers {
export interface GetUsersOut extends BaseResponseOut {
    userOut: UserOut[];
}

export interface DeptWiseUserOut {
    id: number | null;
    name: string | null;
    managerId: number | null;
    manager: ManagerOut | null;
    groups: GroupOut[];
    users: UserOut[];
}

export interface ManagerOut {
    id: number | null;
    name: string | null;
}

export interface GroupOut {
    id: number | null;
    name: string | null;
    groupLeader: GroupLeaderOut | null;
}

export interface GroupLeaderOut {
    id: number | null;
    name: string | null;
}

export interface UserOut {
    id: number | null;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    title: string | null;
    profilePicture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    departmentId: number | null;
    userLevelId: number | null;
    isActive: boolean;
    userModulePermissions: UsersPermissionOut[];
}

export interface UsersPermissionOut {
    id: number;
    permissionId: number | null;
    userId: number | null;
    isView: boolean | null;
    isEdit: boolean | null;
    isUpload: boolean | null;
    isReply: boolean | null;
    isSend: boolean | null;
}

export interface UserLevelOut {
    id: number | null;
    name: string | null;
}

}
export namespace UserAccount.GetUsersByGroupId {
export interface GetUsersByGroupIdIn {
    groupId: number;
}

}
export namespace UserAccount.GetUsersByGroupId {
export interface GetUsersByGroupIdOut extends BaseResponseOut {
    deptWiseUserOut: UsersByGroupIdOut[];
}

export interface UsersByGroupIdOut {
    id: number | null;
    name: string | null;
}

}
export namespace UserAccount.GetUsersForDropdown {
export interface GetUsersForDropdownIn {

}

}
export namespace UserAccount.GetUsersForDropdown {
export interface GetUsersForDropdownOut extends BaseResponseOut {
    adminUsersOuts: AdminUsersOut[];
}

export interface AdminUsersOut {
    id: number | null;
    name: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
}

}
export namespace UserAccount.UpdateUserProfile {
export interface UpdateUserProfileIn {
    userId: number | null;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    title: string | null;
    profilePicture: string | null;
    phoneCountry: string | null;
    phoneNumber: string | null;
    userName: string | null;
    password: string | null;
    userType: number | null;
    incomingTranslationLangage: string | null;
    noTranslateWords: string | null;
    customerId: number | null;
    taxiTransferCommission: number | null;
}

}
export namespace UserAccount.UpdateUserStatus {
export interface UpdateUserStatusIn {
    userId: number;
    isActive: boolean;
}

}
export namespace UserAccount.UpdateUserStatus {
export interface UpdateUserStatusOut extends BaseResponseOut {

}

}
export namespace UserLevels.GetCustomerLevels {
export interface GetCustomerLevelsIn {

}

}
export namespace UserLevels.GetCustomerLevels {
export interface GetCustomerLevelsOut extends BaseResponseOut {
    customerLevelsOut: CustomerLevelsOut[];
}

export interface CustomerLevelsOut {
    id: number;
    levelName: string | null;
    normalizedLevelName: string | null;
}

}
export namespace UserLevels.GetUserLevels {
export interface GetUserLevelsIn {

}

}
export namespace UserLevels.GetUserLevels {
export interface GetUserLevelsOut extends BaseResponseOut {
    userLevelsOut: UserLevelsOut[];
}

export interface UserLevelsOut {
    id: number;
    levelName: string | null;
    normalizedLevelName: string | null;
}

}
export namespace Vonage.DeliveryReceiptSMSWebhook {
export interface GetDeliveryReceiptWebhookIn {
    msisdn: string | null;
    to: string | null;
    network_code: string | null;
    messageId: string | null;
    price: string | null;
    status: string | null;
    scts: string | null;
    err_code: string | null;
    api_key: string | null;
    message_timestamp: string | null;
}

}
export namespace Vonage.InboundWebhook {
export interface GetInboundWebhookIn {
    to: string | null;
    from: string | null;
    channel: string | null;
    message_uuid: string | null;
    text: string | null;
    timestamp: string | null;
    message_type: string | null;
    image: ImageData | null;
    context_status: string | null;
    profile: ProfileData | null;
    video: VideoData | null;
    audio: AudioData | null;
    file: FileData | null;
    file: any | null;
    fileUrl: string | null;
    attachment: string | null;
}

export interface ImageData {
    url: string | null;
    caption: string | null;
}

export interface ProfileData {
    name: string | null;
}

export interface VideoData {
    url: string | null;
    caption: string | null;
}

export interface AudioData {
    url: string | null;
    caption: string | null;
}

export interface FileData {
    url: string | null;
    caption: string | null;
}

}
export namespace Vonage.ReceiveSMSWebhook {
export interface ReceiveSMSWebhookIn {
    api_key: string | null;
    msisdn: string | null;
    to: string | null;
    messageId: string | null;
    text: string | null;
    type: string | null;
    keyword: string | null;
    timestamp: string | null;
    nonce: string | null;
    concat: string | null;
    concat_ref: string | null;
    concat_total: string | null;
    concat_part: string | null;
    data: string | null;
    udh: string | null;
}

}
export namespace Vonage.StatusWebook {
export interface GetStatusWebhookOut extends BaseResponseOut {

}

}
export namespace Vonage.Vonage {
export interface VonageOut extends BaseResponseOut {

}

}
export namespace VonageRecordsLogs.CreateVonageRecordsReport {
export interface CreateVonageRecordsReportIn {
    product: string | null;
    date_start: string;
    date_end: string;
    direction: string | null;
    from: string | null;
    to: string | null;
    customerId: number;
}

export interface VonageRecordReportIn {
    product: string | null;
    account_id: string | null;
    date_start: string;
    date_end: string;
    include_subaccounts: boolean | null;
    callback_url: string | null;
    direction: string | null;
    status: string | null;
    include_message: boolean;
    show_concatenated: boolean;
    network: string | null;
    from: string | null;
    to: string | null;
}

}
export namespace VonageRecordsLogs.CreateVonageRecordsReport {
export interface CreateVonageRecordsReportOut extends BaseResponseOut {
    vonageRecordsReportsOut: VonageRecordsReportOut;
}

export interface VonageRecordsReportOut {
    request_id: string;
    request_status: string;
    receive_time: string;
    _links: Links;
    product: string;
    account_id: string;
    date_start: string;
    date_end: string;
    include_subaccounts: boolean;
    callback_url: string;
    include_message: boolean;
    direction: string;
}

export interface Links {
    self: Self;
}

export interface Self {
    href: string;
}

}
export namespace VonageRecordsLogs.GetVonageRecordReport {
export interface GetVonageRecordReportIn {
    customerId: number;
    product: string;
    direction: string;
    date_start: string;
    date_end: string;
    include_message: boolean;
    show_concatenated: boolean;
    status: string;
}

}
export namespace VonageRecordsLogs.GetVonageRecordReport {
export interface GetVonageRecordReportOut extends BaseResponseOut {
    vonageRecordReports: VonageRecordReports;
}

export interface Self {
    href: string | null;
}

export interface Links {
    self: Self;
}

export interface Record {
    account_id: string;
    message_id: string;
    client_ref: string;
    concatenated: string;
    direction: string;
    from: string;
    to: string;
    network: string;
    network_name: string;
    country: string;
    country_name: string;
    date_received: string;
    date_finalized: string;
    latency: string;
    status: string;
    error_code: string;
    error_code_description: string;
    currency: string;
    total_price: string;
    message_body: string;
}

export interface VonageRecordReports {
    _links: Links;
    request_id: string;
    request_status: string;
    received_at: string;
    price: string;
    currency: string;
    account_id: string;
    ids_not_found: string;
    product: string;
    direction: string;
    include_message: string;
    show_concatenated: string;
    items_count: number;
    records: Record[];
    fileURI: string;
}

}
export namespace VonageRecordsLogs.GetVonageRecordsStatus {
export interface GetVonageRecordsStatusIn {
    request_id: string;
    request_status: string;
    product: string;
    account_id: string;
    date_start: string;
    date_end: string;
    include_subaccounts: boolean;
    callback_url: string;
    receive_time: string;
    start_time: string;
    _links: Links;
    items_count: number;
    direction: string;
    from: string;
    to: string;
    id: string;
    include_message: boolean;
}

export interface Links {
    self: Self;
    download_report: DownloadReport;
}

export interface Self {
    href: string;
}

export interface DownloadReport {
    href: string;
}

}
export namespace VonageRecordsLogs.GetVonageRecordsStatus {
export interface GetVonageRecordsStatusOut extends BaseResponseOut {
    csvContent: WebFileOut;
}

}
export namespace VonageSMS.BuyCustomerVonageNumber {
export interface BuyCustomerVonageNumberIn {
    customerId: number;
    country: string;
    number: string;
}

}
export namespace VonageSMS.BuyCustomerVonageNumber {
export interface BuyCustomerVonageNumberOut extends BaseResponseOut {

}

}
export namespace VonageSMS.CancleCustomerVonageNumber {
export interface CancleCustomerVonageNumberIn {
    customerId: number;
    country: string;
    number: string;
}

}
export namespace VonageSMS.CancleCustomerVonageNumber {
export interface CancleCustomerVonageNumberOut extends BaseResponseOut {

}

}
export namespace VonageSMS.GetAvailableNumbers {
export interface GetAvailableNumbersIn {
    customerId: number | null;
    country: string | null;
    type: string;
    pattern: string | null;
    search_pattern: number;
    features: string | null;
    size: number | null;
    index: number | null;
}

}
export namespace VonageSMS.GetAvailableNumbers {
export interface GetAvailableNumbersOut extends BaseResponseOut {
    smsList: string | null;
}

}
export namespace VonageSMS.GetCustomerOwnNumbers {
export interface GetCustomerOwnNumbersIn {
    customerId: number | null;
    search_pattern: number | null;
    pattern: string | null;
}

}
export namespace VonageSMS.GetCustomerOwnNumbers {
export interface GetCustomerOwnNumbersOut extends BaseResponseOut {
    smsList: string | null;
}

}
export namespace VonageSMS.UpdateCustomerVonageNumber {
export interface UpdateCustomerVonageNumberIn {
    customerId: number;
    country: string;
    number: string;
}

}
export namespace VonageSMS.UpdateCustomerVonageNumber {
export interface UpdateCustomerVonageNumberOut extends BaseResponseOut {

}

}