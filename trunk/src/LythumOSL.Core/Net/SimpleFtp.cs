/*
 * Created by SharpDevelop.
 * User: Agira
 * Date: 07/14/2015
 * Time: 02:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Net;

namespace LythumOSL.Core.Net
{
	/// <summary>
	/// SimpleFtp class 
	/// 
	/// Was writen with help of this example: 
	/// http://www.codeproject.com/Tips/443588/Simple-Csharp-FTP-Class
	/// 
	/// Credits for Author!
	/// </summary>
	public class SimpleFtp : ErrorInfo
	{
		#region Constants
		const bool DefaultUseBinary = true;
		const bool DefaultUsePassive = true;
		const bool DefaultKeepAlive = true;
		const int DefaultBufferSize = 0x1000;

		#endregion
		
		#region Attributes

		#endregion
		
		#region Properties
		
		public string Url { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string RemoteDirectory { get; set; }
		
		public bool UseBinary { get; set; }
		public bool UsePassive { get; set; }
		public bool KeepAlive { get; set; }
		public int BufferSize { get; set; }
		
		#endregion
		
		#region Ctor
		
		SimpleFtp()
		{
			this.UseBinary = DefaultUseBinary;
			this.UsePassive = DefaultUsePassive;
			this.KeepAlive = DefaultKeepAlive;
			this.BufferSize = DefaultBufferSize;
		}
		
		public SimpleFtp(string url, string uid, string pwd, string remoteDir)
			: this()
		{
			Url = url;
			Username = uid;
			Password = pwd;
			RemoteDirectory = remoteDir;
		}
		
		/// <summary>
		/// Ftp
		/// </summary>
		/// <param name="url">
		/// URL could contain folders and etc.
		/// Eg. ftp://blabla.com/bla/bla/ 
		/// But never forget slashes as we won't parse here anything.
		/// Full url will be created simply adding to url file text.
		/// 
		/// </param>
		/// <param name="uid"></param>
		/// <param name="pwd"></param>
		public SimpleFtp(string url, string uid, string pwd)
			: this(url, uid, pwd, string.Empty)
		{
		}

		#endregion
		
		#region Helpers
		protected FtpWebRequest InitRequest (string url)
		{
			// Create FTP request
			FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(url);
			
            // Specify credentials
            request.Credentials = new NetworkCredential(Username, Password);
            
            // Request vars
            request.UseBinary = this.UseBinary;
            request.UsePassive = this.UsePassive;
            request.KeepAlive = this.KeepAlive;
            
            return request;
		}
		#endregion
		
		#region Methods
		
		/// <summary>
		/// File download. 
		/// 
		/// You may specify SimpleFtp::RemoteDirectory 
		/// if you wish to change directories while working with this class object instance
		/// if you don't want to recreate object
		/// </summary>
		/// <param name="localFile"></param>
		/// <param name="remoteFile"></param>
		/// <returns></returns>
		public void Download (string localFile, string remoteFile)
		{
			FtpWebRequest request = null;
			FtpWebResponse response = null;
			FileStream localStream = null;
			Stream responseStream = null;
			
			try
			{
				// Reset error status
				Error();
				
				// Request
				request = InitRequest(
					Url + RemoteDirectory + remoteFile);
	            
	            // Method
	            request.Method = 
	            	WebRequestMethods.Ftp.DownloadFile;
	            
	            // Init response communication
	            response = 
	            	(FtpWebResponse)request.GetResponse();
	            
	            // Get response stream
	            responseStream = response.GetResponseStream();
	            
	            // Open a file stream to write local file
	            localStream = new FileStream(
	            	localFile, FileMode.Create);
	            
	            // Data buffer
	            byte[] buffer = new byte[BufferSize];
	            int bytesRead = responseStream.Read(buffer, 0, BufferSize);
	            
	            // Download the file writing buffered data
	            try
	            {
	                while (bytesRead > 0)
	                {
	                    localStream.Write(buffer, 0, bytesRead);
	                    bytesRead = responseStream.Read(buffer, 0, BufferSize);
	                }
	            }
	            catch (Exception ex) 
	            {
	            	Error(ex);
	            }
	            finally
	            {
		            localStream.Close();
		            responseStream.Close();
		            response.Close();
		            request = null;
	            }
	        }
	        catch (Exception ex) 
	        { 
	        	Error(ex);
	        }
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="remoteFile"></param>
		/// <param name="localFile"></param>
		public void Upload(string remoteFile, string localFile)
	    {
			FtpWebRequest request = null;
			//FtpWebResponse response = null;
			FileStream localStream = null;
			Stream responseStream = null;
			
	        try
	        {
	        	Error();
	        	
	        	// Init request
	        	request = InitRequest(
	        		Url + RemoteDirectory + remoteFile);
	        	
	        	// Type of request
	        	request.Method = WebRequestMethods.Ftp.UploadFile;
	        	
	            // Init response communication
	            responseStream = request.GetRequestStream();
	            
	            // Open a file stream to read the file for upload
	            //localStream = new FileStream(localFile, FileMode.Create);
	            localStream = new FileStream(localFile, FileMode.Open, FileAccess.Read);
	            
	            // Buffer for data
	            byte[] buffer = new byte[BufferSize];
	            int bytesSent = localStream.Read(buffer, 0, BufferSize);
	            
	            // Upload the file sending the buffered data 
	            try
	            {
	                while (bytesSent != 0)
	                {
	                    responseStream.Write(buffer, 0, bytesSent);
	                    bytesSent = localStream.Read(buffer, 0, BufferSize);
	                }
	            }
	            catch (Exception ex) 
	            {
	            	Error(ex);
	            }
	            finally
	            {
	            	// Cleanup
		            localStream.Close();
		            responseStream.Close();
		            //response.Close();
		            request = null;
	            }
	        }
	        catch (Exception ex)
	        { 
	        	Error(ex);
	        }
	    }
		
		/// <summary>
		/// Delete specified file
		/// </summary>
		/// <param name="file"></param>
		public void Delete(string file)
    	{
			FtpWebRequest request = null;
			FtpWebResponse response = null;
				
	        try
	        {
	        	Error();
	        	
	        	request = InitRequest(Url + RemoteDirectory + file);
	        	
	        	request.Method = WebRequestMethods.Ftp.DeleteFile;
	        	
	            /* Establish Return Communication with the FTP Server */
	            response = (FtpWebResponse)request.GetResponse();
	        }
	        catch (Exception ex) 
	        { 
	        	Error(ex);
	        }
	        finally
	        {
	            // cleanup
	            if(response!=null)
	            	response.Close();
	            
	            request = null;
	        }
	    }
	
		
		
		#endregion
		
	}
}
