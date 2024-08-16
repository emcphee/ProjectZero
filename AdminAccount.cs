using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

class AdminAccount
{

    #region Properties

    [Key]
    public int AccountNo { get; set; }
    public string AccountName { get; set; }
    public string AccountPassword { get; set; }
    #endregion

    #region Methods
    public AdminAccount(string AccountName, string AccountPassword)
    {
        this.AccountName = AccountName;
        this.AccountPassword = AccountPassword;
    }
    #endregion
}