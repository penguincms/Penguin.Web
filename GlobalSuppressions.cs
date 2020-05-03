// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>", Scope = "module")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "module")]
[assembly: SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "<Pending>", Scope = "module")]
[assembly: SuppressMessage("Usage", "CA2214:Do not call overridable methods in constructors", Justification = "<Pending>", Scope = "member", Target = "~M:Penguin.Web.HttpInteractionBase.#ctor(System.Byte[])")]
[assembly: SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "<Pending>", Scope = "module")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Penguin.Web.HttpServerInteraction.ToString~System.String")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Penguin.Web.Readers.HttpReader.DecodeBodyBytes(System.Collections.Generic.IEnumerable{System.Byte})~System.Byte[]")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~P:Penguin.Web.HttpServerResponse.StatusCode")]
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>", Scope = "module")]
[assembly: SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "<Pending>", Scope = "module")]
[assembly: SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "<Pending>", Scope = "type", Target = "~T:Penguin.Web.HttpServerCapture")]
