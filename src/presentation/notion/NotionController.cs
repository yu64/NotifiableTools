using System;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Interop;


namespace NotifiableTools;

public partial class NotionController
{
    private readonly INotion notion;

    private readonly NotionWindow window;

    public NotionController(INotion notion)
    {
        this.notion = notion;

        this.window = new NotionWindow(notion);
    }

    public void Close()
    {
        this.window.Close();
    }
    
}