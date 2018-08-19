
//XAML can be optionally compiled directly into intermediate language (IL) with the XAML compiler (XAMLC).
//https://docs.microsoft.com/en-us/xamarin/xamarin-forms/xaml/xamlc
//XAMLC offers a number of a benefits:
//
//    It performs compile-time checking of XAML, notifying the user of any errors.
//    It removes some of the load and instantiation time for XAML elements.
//    It helps to reduce the file size of the final assembly by no longer including .xaml files.

using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
