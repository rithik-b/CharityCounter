using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace CharityCounter.Configuration
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }
        public virtual bool ModEnabled { get; set; } = false;
        public virtual int NotesMissed { get; set; } = 0;
        public virtual float MissWeighting { get; set; } = 0.01f;
        public virtual int MapsFailed { get; set; } = 0;
        public virtual float FailWeighting { get; set; } = 3f;
        public virtual string FileContent { get; set; } = "${dollars}, Misses: {misses}, Maps Failed: {fails}";
        public virtual string ChatCommand { get; set; } = "!charity";
        public virtual string ChatContent { get; set; } = "${dollars}, Misses: {misses}, Maps Failed: {fails}";

        /// <summary>
        /// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
        /// </summary>
        public virtual void OnReload()
        {
            // Do stuff after config is read from disk.
        }

        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed()
        {
            // Do stuff when the config is changed.
        }

        /// <summary>
        /// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
        /// </summary>
        public virtual void CopyFrom(PluginConfig other)
        {
            // This instance's members populated from other
        }
    }
}