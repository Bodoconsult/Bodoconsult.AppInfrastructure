Bodoconsult.Text
===============

# What does the library

Bodoconsult.Text provides tools to generate structured text documents like reports in a C# project and export it finally to a HTML, ASCII plain text file or a PDF file.

We use Bodoconsult.Text for creating unit test logfiles intended for presentation to auditors. Another usage at Bodoconsult is creating an automated documentation for an app based on unit tests.

Bodoconsult.Text provides the following main classes;

> [DocumentBuilder class](#documentbuilder-class): easy setup of a LDML document and rendering it to HTML, PDF etc.

> [StructuredText class](#structuredtext-class): simple structured text generation and rendering it to HTM and PDF

# How to use the library

The source code contains NUnit test classes, the following source code is extracted from. The samples below show the most helpful use cases for the library.

# DocumentBuilder class

LDML is a markup language to define the content of structured text documents with paragraphs, headings, images, figures etc.. With the help of renderers the LDML markup can be translated into file formats like HTML, TXT, RTF, DOCX and PDF. 

Central class for using LDML in C# is the DocumentBuilder class.

For more details see [LDML based document creation](Documents.md).


# StructuredText class

Structured text is a simplified version of handling structured text documents compared to DocumentBuilder class. It is kept mainly for compatibilty reasons.

Define your StructuredText instance as required and then let a ITextFormatter instance do its work and produce the requsted output like plain text, HTML or PDF.

For more details see [StructuredText class based document creation](StructuredText.md).




# About us

Bodoconsult (<http://www.bodoconsult.de>) is a Munich based software company.

Robert Leisner is senior software developer at Bodoconsult. See his profile on <http://www.bodoconsult.de/Curriculum_vitae_Robert_Leisner.pdf>.

