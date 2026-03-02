Development Point Need to Consider :

1. We make Separate Handler for Each Create, Update and Delete , GetAll and GetByID
2. We Need to Pass CancellationToken on Every  Asynchronous Call 
3. Try to Set/Puts Basic Required Validation on Validation File based on your Understanding at this time will add or Remove based on UI team or our future Discussion
4. Before Move to Next API just Check/Test your Last API Work it's have all Required Operation Endpoint or not as Mention Point 1
5. Also If Possible I seen some code pass Different Code for Same Response For e.g Not Found By ID Response some code used AppStatusCodeError.Gone410, and some AppStatusCodeError.Conflict409)
6. Remove Un-Used Namespace form Code file     