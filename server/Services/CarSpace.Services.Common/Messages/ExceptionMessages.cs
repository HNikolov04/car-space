namespace CarSpace.Services.Common.Messages;

public static class ExceptionMessages
{
    public const string ListingNotFound = "The requested listing does not exist.";
    public const string UnauthorizedListingAccess = "You are not authorized to perform this action on this listing.";
    public const string InvalidListingData = "Provided listing data is invalid.";

    public const string InvalidBrandData = "Brand data is invalid.";
    public const string BrandNotFound = "The requested car brand was not found.";

    public const string ContactInfoNotSet = "Contact section is not initialized or available.";

    public const string CategoryNotFound = "Car service category not found.";
    public const string InvalidCategoryName = "Category name cannot be empty.";

    public const string MeetNotFound = "Car meet not found.";
    public const string InvalidMeetData = "Invalid data for creating or updating a car meet.";
    public const string UnauthorizedMeetAccess = "You are not authorized to modify this car meet.";

    public const string CommentNotFound = "Comment not found.";
    public const string InvalidCommentData = "Comment content cannot be empty.";
    public const string UnauthorizedCommentAccess = "You are not authorized to delete this comment.";

    public const string ArticleNotFound = "The requested article was not found.";
    public const string InvalidArticleData = "Article title, description, and brand must be provided.";

    public const string AboutSectionNotFound = "The About Us section has not been initialized.";
    public const string InvalidAboutData = "Title and message must be provided for the About Us section.";

    public const string UserNotFound = "User not found.";
    public const string InvalidLoginCredentials = "Invalid email or password.";
    public const string InvalidUserData = "User data is incomplete or invalid.";
    public const string InvalidPasswordUpdate = "Invalid password update request. Ensure fields are filled and passwords match.";
    public const string InvalidCurrentPassword = "Current password is incorrect.";
    public const string PasswordUpdateFailed = "Password update failed. Ensure it meets complexity requirements.";

    public const string AccessDenied = "Access denied.";
}
