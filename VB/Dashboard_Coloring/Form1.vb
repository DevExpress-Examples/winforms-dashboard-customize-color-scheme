Imports System.Data
Imports System.Drawing
Imports DevExpress.XtraBars.Ribbon
Imports DevExpress.DashboardCommon

Namespace Dashboard_Coloring
	Partial Public Class Form1
		Inherits RibbonForm

		Public Sub New()
			InitializeComponent()
			dashboardDesigner1.CreateRibbon()

			' Create a color table that contains dimension values, measures and colors.
			Dim colorTable As DataTable = CreateColorTable()
			' Load a dashboard from the XML file.
			Dim dashboard As New Dashboard()
			dashboard.LoadFromXml("..\..\Data\Dashboard.xml")
			Dim dataSource As IDashboardDataSource = dashboard.DataSources("dataSource1")

			' Specify the coloring mode for the pie series, pie measures and chart argument.
			Dim pie1 As PieDashboardItem = CType(dashboard.Items("pieDashboardItem1"), PieDashboardItem)
			Dim chart1 As ChartDashboardItem = CType(dashboard.Items("chartDashboardItem1"), ChartDashboardItem)
			pie1.SeriesDimensions(0).ColoringMode = ColoringMode.Hue
			pie1.ColoringOptions.MeasuresColoringMode = ColoringMode.Hue
			chart1.Arguments(0).ColoringMode = ColoringMode.Hue

			For Each row As DataRow In colorTable.Rows
				dashboard.ColorScheme.Add(CreateColorSchemeEntry(row, dataSource, False))
				dashboard.ColorScheme.Add(CreateColorSchemeEntry(row, dataSource, True))
			Next row
			dashboardDesigner1.Dashboard = dashboard
		End Sub

		' Create color scheme entries to map dimension values, measures and colors.
		Private Function CreateColorSchemeEntry(ByVal colorSchemeRecord As DataRow, ByVal dataSource As IDashboardDataSource, ByVal includeMeasures As Boolean) As ColorSchemeEntry
			Dim categoryDefinition As New DimensionDefinition("CategoryName")
			Dim countryDefinition As New DimensionDefinition("Country")
			Dim priceDefinition As New MeasureDefinition("Extended Price")

			Dim entry As New ColorSchemeEntry()
			entry.DimensionKeys.Add(New ColorSchemeDimensionKey(categoryDefinition, colorSchemeRecord("CategoryName")))
			entry.DimensionKeys.Add(New ColorSchemeDimensionKey(countryDefinition, colorSchemeRecord("Country")))
			If includeMeasures Then
				entry.MeasureKey = New ColorSchemeMeasureKey(priceDefinition)
			End If
			entry.ColorDefinition = New ColorDefinition(DirectCast(colorSchemeRecord("color"), Color))
			entry.DataSource = dataSource
			Return entry
		End Function

		Private Function CreateColorTable() As DataTable
			Dim colorTable As New DataTable()
			colorTable.Columns.Add("CategoryName", GetType(String))
			colorTable.Columns.Add("Country", GetType(String))
			colorTable.Columns.Add("measure", GetType(String))
			colorTable.Columns.Add("color", GetType(Color))
			colorTable.Rows.Add("Beverages", "UK", "Extended Price", Color.Red)
			colorTable.Rows.Add("Beverages", "USA", "Extended Price", Color.Green)
			Return colorTable
		End Function
	End Class
End Namespace
