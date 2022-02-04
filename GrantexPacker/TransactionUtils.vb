Imports System.Transactions

Class TransactionUtils
    Public Shared Function CreateTransactionScope(Optional ByVal Isolation As IsolationLevel = IsolationLevel.ReadCommitted) As TransactionScope
        Dim transactionOptions = New TransactionOptions()
        transactionOptions.IsolationLevel = Isolation
        transactionOptions.Timeout = TransactionManager.MaximumTimeout
        Return New TransactionScope(TransactionScopeOption.Required, transactionOptions)

    End Function
End Class