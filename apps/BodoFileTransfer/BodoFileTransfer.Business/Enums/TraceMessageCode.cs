namespace BodoFileTransfer.Business.Enums;

/// <summary>
/// Trace message codes
/// </summary>
public enum TraceMessageCode
{
    ApplicationEvent,
    FileRegistered,
    FileSent,
    FileRegistrationError,
    FileSentError,
    FileArchiveRegistered,
    FileDelete,
    FileReSendRegistered
}