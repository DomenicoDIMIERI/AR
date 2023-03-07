using org.dmdpdf.documents;
using org.dmdpdf.documents.contents.composition;
using org.dmdpdf.documents.contents.entities;
using org.dmdpdf.documents.contents.fonts;
using org.dmdpdf.documents.contents.xObjects;
using org.dmdpdf.documents.interaction.actions;
using org.dmdpdf.documents.interaction.navigation.document;
using org.dmdpdf.files;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace org.dmdpdf.samples.cli
{
  /**
    <summary>This sample demonstrates how to apply actions to a document.</summary>
    <remarks>In this case, on document-opening a go-to-page-2 action is triggered;
    then on page-2-opening a go-to-URI action is triggered.</remarks>
  */
  public class ActionSample
    : Sample
  {
    public override void Run(
      )
    {
      // 1. Opening the PDF file...
      string filePath = PromptFileChoice("Please select a PDF file");
      using(File file = new File(filePath))
      {
        Document document = file.Document;
        Page page = document.Pages[1]; // Page 2 (zero-based index).

        // 2. Applying actions...
        // 2.1. Local go-to.
        /*
          NOTE: This statement instructs the PDF viewer to go to page 2 on document opening.
        */
        document.Actions.OnOpen = new GoToLocal(
          document,
          new LocalDestination(page) // Page 2 (zero-based index).
          );

        // 2.2. Remote go-to.
        try
        {
          /*
            NOTE: This statement instructs the PDF viewer to navigate to the given URI on page 2
            opening.
          */
          page.Actions.OnOpen = new GoToURI(
            document,
            new Uri("http://www.sourceforge.net/projects/clown")
            );
        }
        catch(Exception exception)
        {throw new Exception("Remote goto failed.",exception);}

        // 3. Serialize the PDF file!
        Serialize(file, "Actions", "applying actions", "actions, creation, local goto, remote goto");
      }
    }
  }
}