using System.ComponentModel;

namespace SinoticoCsvService
    {
    /// <summary>
    /// Initializes a new colaboration with the InstallUtil.exe.
    /// </summary>
    /// <seealso cref="System.Configuration.Install.Installer" />
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
        {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectInstaller"/> class.
        /// </summary>
        public ProjectInstaller()
            {
            InitializeComponent();
            }
        }
    }
