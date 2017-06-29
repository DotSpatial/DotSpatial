Option Strict On
Option Explicit On

Public Delegate Function PredicateWrapperDelegate(Of T, A)(ByVal item As T, ByVal argument As A) As Boolean
Public Class PredicateWrapper(Of T, A)

    Private _argument As A

    Private _wrapperDelegate As PredicateWrapperDelegate(Of T, A)



    Public Sub New(ByVal argument As A, ByVal wrapperDelegate As PredicateWrapperDelegate(Of T, A))
        _argument = argument
        _wrapperDelegate = wrapperDelegate
    End Sub



    Private Function InnerPredicate(ByVal item As T) As Boolean
        Return _wrapperDelegate(item, _argument)
    End Function



    Public Shared Widening Operator CType(ByVal wrapper As PredicateWrapper(Of T, A)) As Predicate(Of T)
        Return New Predicate(Of T)(AddressOf wrapper.InnerPredicate)
    End Operator


    'Wherever you want to use Array(Of T).FindAll you call it like this and pass a “finder method” that you define nearby:

    'Public Shared Function GetAgencies(ByVal secretariatCode As String) As Agency()
    '    Dim agList As MyLink() = WSHelper.GetAgencies()
    '    Dim agSearchObj As New MyLink
    '    agSearchObj.SecretariatCode = secretariatCode
    '    Return Array.FindAll(Of MyLink)(agList, New PredicateWrapper(Of Agency, Agency)(agSearchObj, AddressOf AgencyMatch))
    'End Function

End Class
