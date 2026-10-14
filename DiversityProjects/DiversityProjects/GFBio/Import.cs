using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace DiversityProjects.GFBio
{
    public struct GFBioValue
    {
        public string Table;
        public string Column;
        public string Setting;
        public string Identifier;
        public string Value;
    }

    // JSON submission data import class

    public class Import
    {
        #region Constants

        private const string StrLicenseUriCCBY = "https://creativecommons.org/licenses/{0}/";
        private const string StrBackProjectID = "http://id.snsb.info/gfbio/projects_gfbio/{0}";
        private const string StrLicenseDisplayText = "License";
        private const string StrIssueID = "GFBioIssueID";
        private const string StrDatasetID = "GFBioDatasetID";
        private const string StrSubmissionID = "GFBioSubmissionID";
        private const string StrLicenseIPR = "Botanische Staatssammlung München, SNSB";
        private const string StrLicenseHolder = "Staatliche Naturwissenschaftliche Sammlungen Bayerns";
        private const string StrAgentRoleSubmitter = "Submitter";
        private const string StrAgentRoleContributor = "Author";
        private const string StrAgentRolePublisher = "Publisher";
        private const string StrAgentRoleTechnicalContact = "Technical Contact";
        private const string StrAgentRoleContentContact = "Content Contact";

        private const string StrMetadataText = "MetadataDescription";
        private const string StrEmbargoEndsText = "EmbargoEnds";
        private const string StrTimespanText = "Timespan";
        private const string StrLegalRestrictionsText = "LegalRestrictions";
        private const string StrArchive = "StorageLocation";
        private const string StrArchivePath = "RawDataIngest\\";


        #endregion Constants

        #region Fields

        private string m_RequestUri = "https://helpdesk.gfbio.org/rest/api/2/issue/";
        private NetworkCredential m_Credentials = null;
        private string m_Domain = "https://helpdesk.gfbio.org";

        // Submission data
        private string m_Submission_ProjectID = "";

        // Database entries

        // Project:
        private string m_Project_Project = "";
        private string m_Project_ProjectTitle = "";
        private string m_Project_ProjectDescription = "";
        private string m_Project_ProjectVersion = "";
        private List<string> m_ProjectLabel_Label = new List<string>();
        private string m_Project_EmbargoEnds = "";
        // ProjectIdentifier:
        private string m_Submission_IssueID = "";
        private string m_Submission_DatasetID = "";
        private string m_Submission_SubmissionID = "";
        // ProjectLicense:
        private string m_ProjectLicense_LicenseType = "";
        private string m_ProjectLicense_LicenseURI = "";
        // ProjectAgent:
        private string m_ProjectAgent_AgentName = "";
        private string m_ProjectAgent_EMail = "";
        private string m_ProjectAgent_AgentRole = "";
        // Agent:
        private string m_Agent_GivenName = "";
        private string m_Agent_InheritedName = "";
        private string m_Agent_Email = "";
        private string m_Agent_Institution = "";
        private string m_Agent_Contribution = "";
        // ProjectReference:
        private string m_ProjectReference_ReferenceTitle = "";
        // ProjectSetting:
        private List<string> m_ProjectSetting_Metadata = new List<string>();
        private List<string> m_ProjectKeyword_Keyword = new List<string>();
        private string m_ProjectSetting_Timespan = "";
        private List<string> m_ProjectSetting_LegalRestrictions = new List<string>();
        private List<string> m_ProjectSetting_Files = new List<string>();
        private string m_ProjectSetting_Archive = "";


        #endregion Fields

        #region Parameter

        private string m_User;
        private string m_Password;
        private string m_Key;
        private System.Collections.Generic.List<GFBioValue> m_GFBioValues = new List<GFBioValue>();
        private string _DefaultAgentRole = "";

        #endregion Parameter

        #region Construction

        public Import(string User, string Password, string Key, string DefaultRoleForAgents = "")
        {
            m_User = User;
            m_Password = Password;
            m_Key = Key;
            m_GFBioValues = null;
            if (DefaultRoleForAgents.Length > 0)
                this._DefaultAgentRole = DefaultRoleForAgents;
            SetCredentials();
        }

        #endregion Construction

        #region Interface

        public System.Collections.Generic.List<GFBioValue> GFBioValues(string ProjectName = "")
        {
            if (m_GFBioValues == null)
                m_GFBioValues = new List<GFBioValue>();
            ExtractValues(ProjectName);
            return m_GFBioValues;
        }

        #endregion Interface

        #region Methods

        private void SetCredentials()
        {
            // Set Credentials
            m_Credentials = new NetworkCredential(m_User, m_Password, m_Domain);
        }

        // Extract GFBio submission values
        private void ExtractValues(string ProjectName = "")
        {
            try
            {
                // Get Submission Text from REST service
                string response = GetSubmission(m_RequestUri, m_Key);
                string strKey5 = m_Key.Replace("DSUB-", "");
                if (strKey5 != null && strKey5.Length > 0 && strKey5.Length <= 5)
                    strKey5 = "00000".Substring(0, 5 - strKey5.Length) + strKey5;

                // Extract project data from Submission
                string Institution = "SNSB";
                if (DiversityWorkbench.Settings.DatabaseServer != null
                    && DiversityWorkbench.Settings.DatabaseServer.Length > 0
                    && DiversityWorkbench.Settings.DatabaseServer.IndexOf(".") > -1)
                    Institution = DiversityWorkbench.Settings.DatabaseServer.Substring(0, DiversityWorkbench.Settings.DatabaseServer.IndexOf(".")).ToString().ToUpper();
                else
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("DiversityWorkbench.Settings.DatabaseServer missing");
                if (ProjectName.Length > 0)
                    m_Project_Project = ProjectName;
                else
                    m_Project_Project = "GFBio" + DateTime.Now.Year.ToString() + strKey5 + Institution;  //  

                if (response != null && response.Length > 0)
                {
                    RootObject json = CheckResponse(response);
                    if (json != null)
                    {
                        // Assign data to database entries
                        AssignDatabaseEntries(json);
                    }
                    else
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("Generation of json failed");
                    }
                }
                else
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("response from https://helpdesk.gfbio.org/rest/api/2/issue/ for " + m_Key + " missing");
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        // Get submission data from JIRA DB
        public string GetSubmission(string uri, string submissionID)
        {
            string Response = "";
            try
            {
                string URI = uri + submissionID;
                string authorization = m_User + ":" + m_Password;

                WebRequest req = WebRequest.Create(@URI);
                req.Method = "GET";
                req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(authorization));
                HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

                Stream ReceiveStream = resp.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader readStream = new StreamReader(ReceiveStream, encode);
                Response = readStream.ReadToEnd(); ;
                readStream.Close();
                resp.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("GetSubmission Error: " + ex.Message);
            }

            return Response;
        }

        // Parse Submission data
        private RootObject CheckResponse(string response)
        {
            try
            {
                // Ariane changed to System.Text.Json JsonSerializer for .net8
                //var jss = new JavaScriptSerializer();
                //RootObject json = jss.Deserialize<RootObject>(response);
                RootObject json = System.Text.Json.JsonSerializer.Deserialize<RootObject>(response);
                return json;
            }
            catch (Exception ex)
            {
                MessageBox.Show("CheckResponse Error: " + ex.Message);
            }
            return null;
        }

        // Assign results to database entries
        private void AssignDatabaseEntries(RootObject json)
        {
            try
            {
                // Assign results to DB entries:
                m_Submission_ProjectID = json.fields.customfield_10302;                                                 //  4: Project ID (if already set)
                m_Submission_DatasetID = json.fields.customfield_10309;                                                 //  5: Dataset ID
                m_Project_ProjectTitle = json.fields.customfield_10201;                                                 //  7: Dataset Title
                m_Project_ProjectDescription = json.fields.customfield_10208;                                           //  8: Dataset Description
                m_Project_ProjectVersion = json.fields.customfield_10310;                                               //  6: Dataset Version
                m_Submission_SubmissionID = json.fields.customfield_10303;                                              // 19: Submission ID
                //string strKeywords = json.fields.customfield_10313;                                                     // 16: Keywords
                //List<string> labels = json.fields.customfield_10308;                                                    // 10: Dataset Labels
                //if (labels != null)
                //{
                //    if (labels.Count == 1)
                //        m_ProjectLabel_Label = ExtractCommaSeparatedValuesFromString(labels[0], StrSeparatorLabels);
                //    else
                //        m_ProjectLabel_Label = labels;
                //}

                string strLabels = json.fields.customfield_10313;
                if (strLabels != null)
                    m_ProjectLabel_Label = ExtractCommaSeparatedValuesFromString(strLabels, StrSeparatorLabels);

                // 10: Dataset Labels
                if (json.fields.customfield_10202 != null)
                    m_ProjectLicense_LicenseType = json.fields.customfield_10202.value;                                 // 12: Data License / value
                // ---- neu ----
                if (m_ProjectLicense_LicenseType.StartsWith("CC "))
                {
                    if (m_ProjectLicense_LicenseType.EndsWith(".0"))
                    {
                        string licenseType = m_ProjectLicense_LicenseType.Substring(3);
                        licenseType = licenseType.Substring(0, licenseType.Length - 4);
                        string licenseVersion = m_ProjectLicense_LicenseType.Substring(m_ProjectLicense_LicenseType.Length - 3);
                        m_ProjectLicense_LicenseURI = string.Format(StrLicenseUriCCBY, licenseType.ToLower()) + licenseVersion + "/";   // Created License URI
                    }
                    else
                    {
                        string licenseType = m_ProjectLicense_LicenseType.Substring(3);
                        m_ProjectLicense_LicenseURI = string.Format(StrLicenseUriCCBY, licenseType.ToLower()) + "4.0/";                 // Created License URI
                        m_ProjectLicense_LicenseType += " 4.0";                                                                         // Add fixed version number 4.0
                    }
                }

                /*
                if (m_ProjectLicense_LicenseType.Length > 4 && m_ProjectLicense_LicenseType[3] == 'B')
                {
                    string licenseType = m_ProjectLicense_LicenseType.Substring(3);
                    m_ProjectLicense_LicenseURI = string.Format(StrLicenseUriCCBY, licenseType.ToLower());              // Created License URI
                    m_ProjectLicense_LicenseType += " 4.0";                                                             // Add fixed version number 4.0
                }
                */

                m_ProjectReference_ReferenceTitle = json.fields.customfield_10307;                                      // 13: Related Publications
                if (m_ProjectReference_ReferenceTitle != null && m_ProjectReference_ReferenceTitle.Length > 255)
                    m_ProjectReference_ReferenceTitle = m_ProjectReference_ReferenceTitle.Substring(0, 255);

                List<Customfield10229> fields10229 = json.fields.customfield_10229;
                if (fields10229 != null)
                {
                    foreach (Customfield10229 field in fields10229)                                                     //  9: MetaData Description / value  // <=== mehrere ??  
                        m_ProjectSetting_Metadata.Add(field.value);
                }
                //List<string> labels = json.fields.customfield_10308;                                                    // 10: Dataset Labels
                // List<string> keywords = json.fields.customfield_10306 as List<string>;                               // 16: Keywords
                //string strKeywords = json.fields.customfield_10313;                                                     // 16: Keywords
                //if (strKeywords != null)
                //    m_ProjectKeyword_Keyword = ExtractCommaSeparatedValuesFromString(strKeywords, StrSeparatorKeywords);

                List<string> keywords = json.fields.customfield_10308;                                                     // 16: Keywords
                if (keywords != null)
                {
                    if (keywords.Count == 1)
                        m_ProjectKeyword_Keyword = ExtractCommaSeparatedValuesFromString(keywords[0], StrSeparatorKeywords);
                    else
                        m_ProjectKeyword_Keyword = keywords;
                }

                m_Project_EmbargoEnds = json.fields.customfield_10200;                                           // 17: Embargo Ends
                m_ProjectSetting_Timespan = json.fields.customfield_10311;                                              // 9: Data Collection Time
                List<Customfield10216> fields10216 = json.fields.customfield_10216;
                if (fields10216 != null)
                {
                    foreach (Customfield10216 field in fields10216)                                                     // 18: Legal Restrictions / value  // <=== mehrere ??  
                        m_ProjectSetting_LegalRestrictions.Add(field.value);
                }
                List<Attachment> attachments = json.fields.attachment;                                                  // 15: Files 
                if (attachments != null)
                {
                    foreach (Attachment attachment in attachments)
                        m_ProjectSetting_Files.Add(attachment.content);
                }
                // Archive path
                m_ProjectSetting_Archive = StrArchivePath + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString("D2") + "_" + m_Project_Project;

                // Fill up GFBioValues
                m_GFBioValues.Clear();

                // Submitter
                int number = 1;
                string strNumber = number.ToString();
                m_ProjectAgent_AgentName = json.fields.reporter.displayName;                                            // 21: Reporter (displayName)
                m_ProjectAgent_EMail = json.fields.reporter.emailAddress;                                               // 21: Reporter (emailAddress)

                string[] splitAgentNames = m_ProjectAgent_AgentName.Split(StrSeparatorAgentNames, StringSplitOptions.RemoveEmptyEntries);
                if (splitAgentNames.Length > 1)
                {
                    string givenNames = string.Empty;
                    for (int i = 0; i < splitAgentNames.Length - 1; i++)
                    {
                        givenNames += splitAgentNames[i] + " ";
                    }
                    m_Agent_GivenName = givenNames.TrimEnd(Blanks);
                }
                if (splitAgentNames.Length > 0)
                {
                    m_Agent_InheritedName = splitAgentNames[splitAgentNames.Length - 1];
                }
                //m_Agent_Institution = "Staatliche Naturwissenschaftliche Sammlungen Bayerns";
                m_Agent_Institution = GFBio.SettingsGFBio.Default.Submitter; // "Staatliche Naturwissenschaftliche Sammlungen Bayerns";
                m_Agent_Contribution = "";

                // Publisher, Technical Contact
                //m_GFBioValues.Add(CreateGFBioValue("Agent", "GivenName", "", strNumber, "Staatliche Naturwissenschaftliche Sammlungen Bayerns - SNSB IT Center, München"));
                m_GFBioValues.Add(CreateGFBioValue("Agent", "GivenName", "", strNumber, GFBio.SettingsGFBio.Default.Publisher));// "Staatliche Naturwissenschaftliche Sammlungen Bayerns - SNSB IT Center, München"));
                m_GFBioValues.Add(CreateGFBioValue("Agent", "InheritedName", "", strNumber, ""));
                m_GFBioValues.Add(CreateGFBioValue("Agent", "AgentType", "", strNumber, "Institution"));
                m_GFBioValues.Add(CreateGFBioValue("ProjectAgentRole", "AgentRole", "", strNumber, StrAgentRolePublisher));
                m_GFBioValues.Add(CreateGFBioValue("ProjectAgentRole", "AgentRole", "", strNumber, StrAgentRoleTechnicalContact));
                m_GFBioValues.Add(CreateGFBioValue("AgentContactInformation", "Email", "", strNumber, ""));
                m_GFBioValues.Add(CreateGFBioValue("AgentContactInformation", "Institution", "", strNumber, m_Agent_Institution));
                number++;
                strNumber = number.ToString();

                // Submitter, Content Contact
                m_GFBioValues.Add(CreateGFBioValue("Agent", "GivenName", "", strNumber, m_Agent_GivenName));
                m_GFBioValues.Add(CreateGFBioValue("Agent", "InheritedName", "", strNumber, m_Agent_InheritedName));
                m_GFBioValues.Add(CreateGFBioValue("Agent", "AgentType", "", strNumber, "Person"));
                m_GFBioValues.Add(CreateGFBioValue("ProjectAgentRole", "AgentRole", "", strNumber, StrAgentRoleSubmitter));
                m_GFBioValues.Add(CreateGFBioValue("ProjectAgentRole", "AgentRole", "", strNumber, StrAgentRoleContentContact));
                m_GFBioValues.Add(CreateGFBioValue("AgentContactInformation", "Email", "", strNumber, m_ProjectAgent_EMail));
                m_GFBioValues.Add(CreateGFBioValue("AgentContactInformation", "Institution", "", strNumber, m_Agent_Institution));

                // new: Agent(s)
                // 
                string agents = json.fields.customfield_10205 as string;
                string[] splitAgents = agents.Split(StrSeparatorAgents, StringSplitOptions.RemoveEmptyEntries);

                foreach (string agent in splitAgents)
                {
                    number++;
                    strNumber = number.ToString();
                    string[] splitAgent = agent.Split(StrSeparatorNames, StringSplitOptions.RemoveEmptyEntries);
                    if (splitAgent.Length > 0)
                    {
                        m_Agent_InheritedName = splitAgent[0];
                        m_GFBioValues.Add(CreateGFBioValue("Agent", "AgentType", "", strNumber, "Person"));
                    }
                    m_GFBioValues.Add(CreateGFBioValue("Agent", "InheritedName", "", strNumber, m_Agent_InheritedName));
                    if (splitAgent.Length > 1)
                    {
                        m_Agent_GivenName = splitAgent[1];
                        m_GFBioValues.Add(CreateGFBioValue("Agent", "GivenName", "", strNumber, m_Agent_GivenName));
                    }
                    if (splitAgent.Length > 2)
                    {
                        m_Agent_Email = splitAgent[2];
                        m_GFBioValues.Add(CreateGFBioValue("AgentContactInformation", "Email", "", strNumber, m_Agent_Email));
                    }
                    if (splitAgent.Length > 3)
                    {
                        m_Agent_Institution = splitAgent[3];
                        m_GFBioValues.Add(CreateGFBioValue("AgentContactInformation", "Institution", "", strNumber, m_Agent_Institution));
                    }
                    if (splitAgent.Length > 4)
                    {
                        string[] splitAgentRoles = splitAgent[4].Split(StrSeparatorKeywords, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string role in splitAgentRoles)
                        {
                            // Convert GFBio roles to DP roles
                            string strRole1 = role.Replace("Author/Creator", "Author");
                            string strRole2 = strRole1.Replace("Data Editor/Data Manager", "Data Editor");
                            string strRole3 = strRole2.Replace("Data Source Organisation", "Source Organisation");
                            m_GFBioValues.Add(CreateGFBioValue("ProjectAgentRole", "AgentRole", "", strNumber, strRole3));

                        }
                    }
                    else if (this._DefaultAgentRole.Length > 0)
                    {
                        m_GFBioValues.Add(CreateGFBioValue("ProjectAgentRole", "AgentRole", "", strNumber, this._DefaultAgentRole));
                    }
                }

                // Project
                m_GFBioValues.Add(CreateGFBioValue("Project", "Project", "", "", m_Project_Project));
                m_GFBioValues.Add(CreateGFBioValue("Project", "ProjectTitle", "", "", m_Project_ProjectTitle));
                m_GFBioValues.Add(CreateGFBioValue("Project", "ProjectDescription", "", "", m_Project_ProjectDescription));
                m_GFBioValues.Add(CreateGFBioValue("Project", "ProjectVersion", "", "", m_Project_ProjectVersion));
                // ProjectLabel
                foreach (string label in m_ProjectLabel_Label)
                    m_GFBioValues.Add(CreateGFBioValue("ProjectLabel", "Label", "", "", label));
                // ProjectIdentifier
                m_GFBioValues.Add(CreateGFBioValue("ProjectIdentifier", "Type", "", m_Key, StrIssueID));
                string url = m_RequestUri.Replace("rest/api/2/issue", "browse") + m_Key;
                m_GFBioValues.Add(CreateGFBioValue("ProjectIdentifier", "URL", "", m_Key, url));
                m_GFBioValues.Add(CreateGFBioValue("ProjectIdentifier", "Type", "", m_Submission_DatasetID, StrDatasetID));
                m_GFBioValues.Add(CreateGFBioValue("ProjectIdentifier", "Type", "", m_Submission_SubmissionID, StrSubmissionID));
                // ProjectLicense
                m_GFBioValues.Add(CreateGFBioValue("ProjectLicense", "DisplayText", "", "", StrLicenseDisplayText));
                m_GFBioValues.Add(CreateGFBioValue("ProjectLicense", "LicenseType", "", "", m_ProjectLicense_LicenseType));
                // m_GFBioValues.Add(CreateGFBioValue("ProjectLicense", "LicenseHolder", "", StrLicenseHolder));
                // m_GFBioValues.Add(CreateGFBioValue("ProjectLicense", "IPR", "", StrLicenseIPR));
                m_GFBioValues.Add(CreateGFBioValue("ProjectLicense", "LicenseURI", "", "", m_ProjectLicense_LicenseURI));
                // ProjectAgent --> now treated as Agent!
                // m_GFBioValues.Add(CreateGFBioValue("ProjectAgent", "AgentName", "", "", m_ProjectAgent_AgentName));
                // m_GFBioValues.Add(CreateGFBioValue("ProjectAgentRole", "AgentRole", m_ProjectAgent_AgentName, "", m_ProjectAgent_AgentRole));
                // ProjectReference
                m_GFBioValues.Add(CreateGFBioValue("ProjectReference", "ReferenceTitle", "", "", m_ProjectReference_ReferenceTitle));
                // ProjectResource
                foreach (string file in m_ProjectSetting_Files)
                    m_GFBioValues.Add(CreateGFBioValue("ProjectResource", "URI", "", "", file));
                // ProjectKeyword
                foreach (string keyword in m_ProjectKeyword_Keyword)
                    m_GFBioValues.Add(CreateGFBioValue("ProjectKeyword", "Keyword", "", "", keyword));
                // ProjectSetting
                int index = 1;
                foreach (string metadata in m_ProjectSetting_Metadata)
                {
                    if (m_ProjectSetting_Metadata.Count > 1)
                        m_GFBioValues.Add(CreateGFBioValue("ProjectSetting", "Value", StrMetadataText + (index++).ToString(), "", metadata));
                    else
                        m_GFBioValues.Add(CreateGFBioValue("ProjectSetting", "Value", StrMetadataText, "", metadata));
                }
                m_GFBioValues.Add(CreateGFBioValue("Project", "EmbargoDate", "", "", m_Project_EmbargoEnds));
                // m_GFBioValues.Add(CreateGFBioValue("ProjectSetting", "Value", StrTimespanText, "", m_ProjectSetting_Timespan));
                index = 1;
                foreach (string restriction in m_ProjectSetting_LegalRestrictions)
                {
                    if (m_ProjectSetting_LegalRestrictions.Count > 1)
                        m_GFBioValues.Add(CreateGFBioValue("ProjectSetting", "Value", StrLegalRestrictionsText + (index++).ToString(), "", restriction));
                    else
                        m_GFBioValues.Add(CreateGFBioValue("ProjectSetting", "Value", StrLegalRestrictionsText, "", restriction));
                }
                // Archive directory
                // Check presence of table
                bool ProjectArchiveExists = false;
                string SQL = "select count(*) from INFORMATION_SCHEMA.TABLES T where T.TABLE_NAME = 'ProjectArchive'";
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result == "1")
                    m_GFBioValues.Add(CreateGFBioValue("ProjectArchive", StrArchive, "", "", m_ProjectSetting_Archive));
            }
            catch (Exception ex)
            {
                MessageBox.Show("AssignDatabaseEntries Error: " + ex.Message);
            }
        }

        private string[] StrSeparatorKeywords = new string[] { "," };
        private string[] StrSeparatorNames = new string[] { "|" };
        private string[] StrSeparatorAgents = new string[] { "\r\n", "\n" };
        private string[] StrSeparatorAgentNames = new string[] { " ", "," };
        private string[] StrSeparatorLabels = new string[] { ";", "," };
        private char[] Blanks = new char[] { ' ' };

        private List<string> ExtractCommaSeparatedValuesFromString(string input, string[] separator)
        {
            List<string> output = new List<string>();
            try
            {
                string[] splitResult = input.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in splitResult)
                {
                    string str1 = str.Replace('_', ' ');
                    string str2 = str1.TrimStart(Blanks);
                    string str3 = str2.TrimEnd(Blanks);
                    output.Add(str3);
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return output;
        }

        private GFBioValue CreateGFBioValue(string table, string column, string setting, string identifier, string value)
        {
            GFBioValue gfbioValue = new GFBioValue();
            gfbioValue.Table = table;
            gfbioValue.Column = column;
            gfbioValue.Setting = setting;
            gfbioValue.Identifier = identifier;
            gfbioValue.Value = value;
            return gfbioValue;
        }

        #endregion Methods
    }

    // GFBio Submission format as classes

    #region Classes etc.
    public class Customfield10229
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Priority
    {
        public string self { get; set; }
        public string iconUrl { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
    }

    public class StartTime
    {
        public object iso8601 { get; set; }
        public object jira { get; set; }
        public string friendly { get; set; }
        public object epochMillis { get; set; }
    }

    public class StopTime
    {
        public object iso8601 { get; set; }
        public object jira { get; set; }
        public string friendly { get; set; }
        public object epochMillis { get; set; }
    }

    public class GoalDuration
    {
        public object millis { get; set; }
        public string friendly { get; set; }
    }

    public class ElapsedTime
    {
        public object millis { get; set; }
        public string friendly { get; set; }
    }

    public class RemainingTime
    {
        public object millis { get; set; }
        public string friendly { get; set; }
    }

    public class CompletedCycle
    {
        public StartTime startTime { get; set; }
        public StopTime stopTime { get; set; }
        public object breached { get; set; }
        public GoalDuration goalDuration { get; set; }
        public ElapsedTime elapsedTime { get; set; }
        public RemainingTime remainingTime { get; set; }
    }

    public class Customfield10101
    {
        public string id { get; set; }
        public string name { get; set; }
        public Links _links { get; set; }
        public List<CompletedCycle> completedCycles { get; set; }
    }

    public class Customfield10216
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class AvatarUrls
    {
        public string __invalid_name__48x48 { get; set; }
        public string __invalid_name__24x24 { get; set; }
        public string __invalid_name__16x16 { get; set; }
        public string __invalid_name__32x32 { get; set; }
    }

    public class Assignee
    {
        public string self { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string emailAddress { get; set; }
        public AvatarUrls avatarUrls { get; set; }
        public string displayName { get; set; }
        public object active { get; set; }
        public string timeZone { get; set; }
    }

    public class StatusCategory
    {
        public string self { get; set; }
        public object id { get; set; }
        public string key { get; set; }
        public string colorName { get; set; }
        public string name { get; set; }
    }

    public class Status
    {
        public string self { get; set; }
        public string description { get; set; }
        public string iconUrl { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public StatusCategory statusCategory { get; set; }
    }

    public class AvatarUrls2
    {
        public string __invalid_name__48x48 { get; set; }
        public string __invalid_name__24x24 { get; set; }
        public string __invalid_name__16x16 { get; set; }
        public string __invalid_name__32x32 { get; set; }
    }

    public class Creator
    {
        public string self { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string emailAddress { get; set; }
        public AvatarUrls2 avatarUrls { get; set; }
        public string displayName { get; set; }
        public object active { get; set; }
        public string timeZone { get; set; }
    }

    public class AvatarUrls3
    {
        public string __invalid_name__48x48 { get; set; }
        public string __invalid_name__24x24 { get; set; }
        public string __invalid_name__16x16 { get; set; }
        public string __invalid_name__32x32 { get; set; }
    }

    public class Reporter
    {
        public string self { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string emailAddress { get; set; }
        public AvatarUrls3 avatarUrls { get; set; }
        public string displayName { get; set; }
        public object active { get; set; }
        public string timeZone { get; set; }
    }

    public class Aggregateprogress
    {
        public object progress { get; set; }
        public object total { get; set; }
    }

    public class Customfield10202
    {
        public string self { get; set; }
        public string value { get; set; }
        public string id { get; set; }
    }

    public class Progress
    {
        public object progress { get; set; }
        public object total { get; set; }
    }

    public class Worklog
    {
        public object startAt { get; set; }
        public object maxResults { get; set; }
        public object total { get; set; }
        public List<object> worklogs { get; set; }
    }

    public class Issuetype
    {
        public string self { get; set; }
        public string id { get; set; }
        public string description { get; set; }
        public string iconUrl { get; set; }
        public string name { get; set; }
        public object subtask { get; set; }
        public object avatarId { get; set; }
    }

    public class AvatarUrls4
    {
        public string __invalid_name__48x48 { get; set; }
        public string __invalid_name__24x24 { get; set; }
        public string __invalid_name__16x16 { get; set; }
        public string __invalid_name__32x32 { get; set; }
    }

    public class ImportProject
    {
        public string self { get; set; }
        public string id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public AvatarUrls4 avatarUrls { get; set; }
    }

    public class Watches
    {
        public string self { get; set; }
        public object watchCount { get; set; }
        public object isWatching { get; set; }
    }

    public class Links2
    {
        public string jiraRest { get; set; }
        public string web { get; set; }
        public string self { get; set; }
    }

    public class Links3
    {
        public string self { get; set; }
    }

    public class IconUrls
    {
        public string __invalid_name__48x48 { get; set; }
        public string __invalid_name__24x24 { get; set; }
        public string __invalid_name__16x16 { get; set; }
        public string __invalid_name__32x32 { get; set; }
    }

    public class Links4
    {
        public IconUrls iconUrls { get; set; }
    }

    public class Icon
    {
        public string id { get; set; }
        public Links4 _links { get; set; }
    }

    public class RequestType
    {
        public string id { get; set; }
        public Links3 _links { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string helpText { get; set; }
        public string serviceDeskId { get; set; }
        public List<string> groupIds { get; set; }
        public Icon icon { get; set; }
    }

    public class StatusDate
    {
        public object iso8601 { get; set; }
        public object jira { get; set; }
        public string friendly { get; set; }
        public object epochMillis { get; set; }
    }

    public class CurrentStatus
    {
        public string status { get; set; }
        public StatusDate statusDate { get; set; }
    }

    public class Customfield10010
    {
        public Links2 _links { get; set; }
        public RequestType requestType { get; set; }
        public CurrentStatus currentStatus { get; set; }
    }

    public class Timetracking
    {
    }

    public class AvatarUrls5
    {
        public string __invalid_name__48x48 { get; set; }
        public string __invalid_name__24x24 { get; set; }
        public string __invalid_name__16x16 { get; set; }
        public string __invalid_name__32x32 { get; set; }
    }

    public class Author
    {
        public string self { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string emailAddress { get; set; }
        public AvatarUrls5 avatarUrls { get; set; }
        public string displayName { get; set; }
        public object active { get; set; }
        public string timeZone { get; set; }
    }

    public class Attachment
    {
        public string self { get; set; }
        public string id { get; set; }
        public string filename { get; set; }
        public Author author { get; set; }
        public object created { get; set; }
        public object size { get; set; }
        public string mimeType { get; set; }
        public string content { get; set; }
        public string thumbnail { get; set; }
    }

    public class AvatarUrls6
    {
        public string __invalid_name__48x48 { get; set; }
        public string __invalid_name__24x24 { get; set; }
        public string __invalid_name__16x16 { get; set; }
        public string __invalid_name__32x32 { get; set; }
    }

    public class Author2
    {
        public string self { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string emailAddress { get; set; }
        public AvatarUrls6 avatarUrls { get; set; }
        public string displayName { get; set; }
        public object active { get; set; }
        public string timeZone { get; set; }
    }

    public class AvatarUrls7
    {
        public string __invalid_name__48x48 { get; set; }
        public string __invalid_name__24x24 { get; set; }
        public string __invalid_name__16x16 { get; set; }
        public string __invalid_name__32x32 { get; set; }
    }

    public class UpdateAuthor
    {
        public string self { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public string emailAddress { get; set; }
        public AvatarUrls7 avatarUrls { get; set; }
        public string displayName { get; set; }
        public object active { get; set; }
        public string timeZone { get; set; }
    }

    public class Comment2
    {
        public string self { get; set; }
        public string id { get; set; }
        public Author2 author { get; set; }
        public string body { get; set; }
        public UpdateAuthor updateAuthor { get; set; }
        public object created { get; set; }
        public object updated { get; set; }
    }

    public class Comment
    {
        public List<Comment2> comments { get; set; }
        public object maxResults { get; set; }
        public object total { get; set; }
        public object startAt { get; set; }
    }

    public class Fields
    {
        public object customfield_10230 { get; set; }
        public List<object> fixVersions { get; set; }
        public object customfield_10231 { get; set; }
        public object customfield_10232 { get; set; }
        public object resolution { get; set; }
        public object customfield_10233 { get; set; }
        public object customfield_10234 { get; set; }
        public object customfield_10500 { get; set; }
        public object customfield_10225 { get; set; }
        public object customfield_10227 { get; set; }
        public object customfield_10228 { get; set; }
        public object customfield_10900 { get; set; }
        public List<Customfield10229> customfield_10229 { get; set; }
        public List<object> customfield_10901 { get; set; }
        public object customfield_10902 { get; set; }
        public object customfield_10903 { get; set; }
        public object lastViewed { get; set; }
        public object customfield_10220 { get; set; }
        public Priority priority { get; set; }
        public object customfield_10100 { get; set; }
        public object customfield_10221 { get; set; }
        public Customfield10101 customfield_10101 { get; set; }
        public object customfield_10222 { get; set; }
        public object customfield_10223 { get; set; }
        public List<string> labels { get; set; }
        public object customfield_10224 { get; set; }
        public object customfield_10214 { get; set; }
        public object customfield_10215 { get; set; }
        public List<Customfield10216> customfield_10216 { get; set; }
        public object customfield_10217 { get; set; }
        public object customfield_10218 { get; set; }
        public object timeestimate { get; set; }
        public object aggregatetimeoriginalestimate { get; set; }
        public List<object> versions { get; set; }
        public List<object> issuelinks { get; set; }
        public Assignee assignee { get; set; }
        public Status status { get; set; }
        public List<object> components { get; set; }
        public object customfield_10210 { get; set; }
        public object customfield_10203 { get; set; }
        public object customfield_10600 { get; set; }
        public object customfield_10204 { get; set; }
        public object customfield_10205 { get; set; }
        public object customfield_10206 { get; set; }
        public object customfield_10207 { get; set; }
        public object aggregatetimeestimate { get; set; }
        public string customfield_10208 { get; set; }
        public object customfield_10209 { get; set; }
        public Creator creator { get; set; }
        public List<object> subtasks { get; set; }
        public Reporter reporter { get; set; }
        public Aggregateprogress aggregateprogress { get; set; }
        public string customfield_10200 { get; set; }
        public string customfield_10201 { get; set; }
        public Customfield10202 customfield_10202 { get; set; }
        public string customfield_10313 { get; set; }
        public object customfield_10314 { get; set; }
        public object customfield_10315 { get; set; }
        public Progress progress { get; set; }
        public Worklog worklog { get; set; }
        public Issuetype issuetype { get; set; }
        public object timespent { get; set; }
        public ImportProject project { get; set; }
        public object aggregatetimespent { get; set; }
        public string customfield_10310 { get; set; }
        public string customfield_10311 { get; set; }
        public object customfield_10312 { get; set; }
        public string customfield_10302 { get; set; }
        public string customfield_10303 { get; set; }
        public object customfield_10700 { get; set; }
        public object customfield_10304 { get; set; }
        public object customfield_10305 { get; set; }
        public object customfield_10306 { get; set; }
        public string customfield_10307 { get; set; }
        public List<string> customfield_10308 { get; set; }
        public object resolutiondate { get; set; }
        public string customfield_10309 { get; set; }
        public object workratio { get; set; }
        public Watches watches { get; set; }
        public object created { get; set; }
        public object customfield_10300 { get; set; }
        public object customfield_10301 { get; set; }
        public object updated { get; set; }
        public object timeoriginalestimate { get; set; }
        public object description { get; set; }
        public Customfield10010 customfield_10010 { get; set; }
        public List<object> customfield_10011 { get; set; }
        public Timetracking timetracking { get; set; }
        public object customfield_10800 { get; set; }
        public List<Attachment> attachment { get; set; }
        public string summary { get; set; }
        public object customfield_10000 { get; set; }
        public object customfield_10001 { get; set; }
        public object customfield_10236 { get; set; }
        public object environment { get; set; }
        public object duedate { get; set; }
        public Comment comment { get; set; }
    }

    public class RootObject
    {
        public string expand { get; set; }
        public string id { get; set; }
        public string self { get; set; }
        public string key { get; set; }
        public Fields fields { get; set; }
    } 
    #endregion

}
