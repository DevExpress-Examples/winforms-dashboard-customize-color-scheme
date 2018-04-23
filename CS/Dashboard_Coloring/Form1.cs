using System.Data;
using System.Drawing;
using DevExpress.XtraBars.Ribbon;
using DevExpress.DashboardCommon;

namespace Dashboard_Coloring {
    public partial class Form1 : RibbonForm {
        public Form1() {
            InitializeComponent();
            dashboardDesigner1.CreateRibbon();

            // Creates a color table containing dimension values, measures and corresponding colors.
            DataTable colorTable = CreateColorTable();
            // Loads a dashboard from the XML file.
            Dashboard dashboard = new Dashboard(); dashboard.LoadFromXml(@"..\..\Data\Dashboard.xml");
            IDashboardDataSource dataSource = dashboard.DataSources["dataSource1"];

            // Specifies the coloring mode for the specified pie series, pie measures and chart argument.
            PieDashboardItem pie1 = (PieDashboardItem)dashboard.Items["pieDashboardItem1"];
            ChartDashboardItem chart1 = (ChartDashboardItem)dashboard.Items["chartDashboardItem1"];
            pie1.SeriesDimensions[0].ColoringMode = ColoringMode.Hue;
            pie1.ColoringOptions.MeasuresColoringMode = ColoringMode.Hue;
            chart1.Arguments[0].ColoringMode = ColoringMode.Hue;

            foreach (DataRow row in colorTable.Rows) {
                dashboard.ColorScheme.Add(CreateColorSchemeEntry(row, dataSource, false));
                dashboard.ColorScheme.Add(CreateColorSchemeEntry(row, dataSource, true));
            }
            dashboardDesigner1.Dashboard = dashboard;
        }

        // Creates color scheme entries used to map dimension values, measures and colors.
        private ColorSchemeEntry CreateColorSchemeEntry(DataRow colorSchemeRecord, 
                                                        IDashboardDataSource dataSource, 
                                                        bool includeMeasures) {
            DimensionDefinition categoryDefinition = new DimensionDefinition("CategoryName");
            DimensionDefinition countryDefinition = new DimensionDefinition("Country");
            MeasureDefinition priceDefinition = new MeasureDefinition("Extended Price");

            ColorSchemeEntry entry = new ColorSchemeEntry();
            entry.DimensionKeys.Add(new ColorSchemeDimensionKey(categoryDefinition, 
                colorSchemeRecord["CategoryName"]));
            entry.DimensionKeys.Add(new ColorSchemeDimensionKey(countryDefinition, 
                colorSchemeRecord["Country"]));
            if(includeMeasures)
                entry.MeasureKey = new ColorSchemeMeasureKey(priceDefinition);
            entry.ColorDefinition = new ColorDefinition((Color)colorSchemeRecord["color"]);
            entry.DataSource = dataSource;
            return entry;
        }

        private DataTable CreateColorTable() {
            DataTable colorTable = new DataTable();
            colorTable.Columns.Add("CategoryName", typeof(string));
            colorTable.Columns.Add("Country", typeof(string));
            colorTable.Columns.Add("measure", typeof(string));
            colorTable.Columns.Add("color", typeof(Color));
            colorTable.Rows.Add("Beverages", "UK", "Extended Price", Color.Red);
            colorTable.Rows.Add("Beverages", "USA", "Extended Price", Color.Green);
            return colorTable;
        }
    }
}
