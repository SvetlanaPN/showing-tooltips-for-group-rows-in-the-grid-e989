Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.Utils

Namespace DevExpress.XtraGrid.Views.ToolTip

	Public Delegate Sub GridViewGroupToolTipControllerShowControlInfo(ByVal value As Object, <System.Runtime.InteropServices.Out()> ByRef text As String, <System.Runtime.InteropServices.Out()> ByRef title As String)

	Public Class GridViewGroupToolTipController
		Private view_Renamed As GridView
		Private columns_Renamed As Dictionary(Of GridColumn, GridViewGroupToolTipControllerShowControlInfo)
		Private tooltip_Renamed As ToolTipController

		'Use Default Tooltip controller
		Public Sub New(ByVal view As GridView)
			Me.New(view, Nothing)
		End Sub
		Public Sub New(ByVal view As GridView, ByVal tooltip As ToolTipController)
			Me.view_Renamed = view
			Me.columns_Renamed = New Dictionary(Of GridColumn, GridViewGroupToolTipControllerShowControlInfo)()
			Me.tooltip_Renamed = tooltip
			AddHandler Me.View.MouseLeave, AddressOf OnView_MouseLeave
			AddHandler Me.View.MouseMove, AddressOf OnView_MouseMove
		End Sub
		Public Sub AddColumnGroupRowToolTip(ByVal column As GridColumn, ByVal toolTipCallback As GridViewGroupToolTipControllerShowControlInfo)
			Columns(column) = toolTipCallback
		End Sub
		Private Sub OnView_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
			Dim hitInfo As GridHitInfo = View.CalcHitInfo(e.X, e.Y)
			If (Not View.IsGroupRow(hitInfo.RowHandle)) Then
				ToolTip.HideHint()
			Else
				Dim column As GridColumn = GetColumnByGroupRowHandle(hitInfo.RowHandle)
				If column IsNot Nothing AndAlso Columns.ContainsKey(column) Then
					Dim value As Object = View.GetGroupRowValue(hitInfo.RowHandle)
					Dim text, title As String
					Columns(column)(value, text, title)
					ToolTip.ShowHint(New DevExpress.Utils.ToolTipControlInfo(hitInfo.RowHandle, text, title))
				Else
					ToolTip.HideHint()
				End If
			End If

		End Sub
		Private Function GetColumnByGroupRowHandle(ByVal rowHandle As Integer) As GridColumn
			Return View.GroupedColumns(View.GetRowLevel(rowHandle))
		End Function
		Private Sub OnView_MouseLeave(ByVal sender As Object, ByVal e As EventArgs)
			ToolTip.HideHint()
		End Sub
		Public ReadOnly Property View() As GridView
			Get
				Return view_Renamed
			End Get
		End Property
		Protected ReadOnly Property Columns() As Dictionary(Of GridColumn, GridViewGroupToolTipControllerShowControlInfo)
			Get
				Return columns_Renamed
			End Get
		End Property
		Protected ReadOnly Property ToolTip() As ToolTipController
			Get
				If tooltip_Renamed IsNot Nothing Then
					Return tooltip_Renamed
				Else
					Return ToolTipController.DefaultController
				End If
			End Get
		End Property
	End Class
End Namespace
