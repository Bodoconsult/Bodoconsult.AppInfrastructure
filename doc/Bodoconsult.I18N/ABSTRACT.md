The basic idea of Bodoconsult.I18N library is to simplify the usage of multiple technologies existing for localization like RESX resources and XAML resource dictionary under one roof.

Bodoconsult.I18N library currently supports I18N for apps with one currently loaded language at a time only but this language can be changed during runtime. Apps with multiple UI threads running on different languages are not supported currently.

Our implementation was intended for using it with console apps, WinForms apps or WPF apps under .Net10 or later. All other use cases may work but are not tested.