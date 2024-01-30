using System;
using System.Collections.Generic;
using System.IO;
using VMFLib.VClass;

namespace VMFLib.Parsers
{
    public class VClassReader : IDisposable
    {
        private StreamReader _reader;

        public VClassReader(string VmfPath)
        {
            _reader = new StreamReader(VmfPath);
            
        }

        public virtual void Dispose()
        {
            _reader.Dispose();
        }

        public virtual BaseVClass ReadClass()
        {
            //TODO: Parse comments correctly
            string line = _reader.ReadLine();
            while (line != null)
            {
                switch (line.Trim())
                {
                    //TODO: Cordons
                    case "cameras":
                    {
                        return ReadCameras();
                    }
                    case "entity":
                    {
                        return ReadEntity();
                    }
                    case "versioninfo":
                    {
                        return ReadVersionInfo();
                    }
                    case "visgroups":
                    {
                        return ReadVisGroups();
                    }
                    case "viewsettings":
                    {
                        return ReadViewSettings();
                    }
                    case "world":
                    {
                        return ReadWorld();
                    }
                    default:
                        line = _reader.ReadLine();
                        continue; //TODO: Better error handling, this will continue until it finds a proper class
                }
            }

            return null;
        }

        #region Editor Info

        public VersionInfo ReadVersionInfo()
        {
            VersionInfo versionInfo = new VersionInfo();
            
            //TODO HACK: We don't want to read the header and bracket but sometimes we only need to skip over the bracket, so this solves the issue
            //This will also skip over comments
            string line = _reader.ReadLine();
            while (line != "{")
            {
                line = _reader.ReadLine();
            }
            line = _reader.ReadLine();
            
            while (line != "}")
            {
                if (line.StartsWith("//") || string.IsNullOrEmpty(line))
                {
                    line = _reader.ReadLine();
                    continue;
                }
                
                versionInfo.AddProperty(new VProperty(line));
                line = _reader.ReadLine();
            }

            return versionInfo;
        }

        public ViewSettings ReadViewSettings()
        {
            ViewSettings viewSettings = new ViewSettings();
            
            //TODO HACK: We don't want to read the header and bracket but sometimes we only need to skip over the bracket, so this solves the issue
            //This will also skip over comments
            string line = _reader.ReadLine();
            while (line != "{")
            {
                line = _reader.ReadLine();
            }
            line = _reader.ReadLine();
            
            while (line != "}")
            {
                if (line.StartsWith("//") || string.IsNullOrEmpty(line))
                {
                    line = _reader.ReadLine();
                    continue;
                }
                
                viewSettings.AddProperty(new VProperty(line));
                line = _reader.ReadLine();
            }

            return viewSettings;
        }

        public CamerasHolder ReadCameras()
        {
            CamerasHolder cameras = new CamerasHolder();
            
            //TODO HACK: We don't want to read the header and bracket but sometimes we only need to skip over the bracket, so this solves the issue
            //This will also skip over comments
            string line = _reader.ReadLine();
            while (line != "{")
            {
                line = _reader.ReadLine();
            }
            line = _reader.ReadLine();
            
            while (line != "}")
            {
                if (line.StartsWith("//") || string.IsNullOrEmpty(line))
                {
                    line = _reader.ReadLine();
                    continue;
                }

                switch (line.Trim())
                {
                    case "camera":
                    {
                        cameras.Cameras.Add(ReadCamera());
                    } break;
                    default:
                    {
                        cameras.AddProperty(new VProperty(line));
                    } break;
                }
                
                line = _reader.ReadLine();
            }

            return cameras;
        }
        
        private Camera ReadCamera()
        {
            Camera camera = new Camera();
            
            //TODO HACK: We don't want to read the header and bracket but sometimes we only need to skip over the bracket, so this solves the issue
            //This will also skip over comments
            string line = _reader.ReadLine();
            while (line != "{")
            {
                line = _reader.ReadLine();
            }
            line = _reader.ReadLine().Trim();
            
            while (line != "}")
            {
                if (line.StartsWith("//") || string.IsNullOrEmpty(line))
                {
                    line = _reader.ReadLine().Trim();
                    continue;
                }
                
                camera.AddProperty(new VProperty(line));
                line = _reader.ReadLine().Trim();
            }

            return camera;
        }
        
        private Editor ReadEditor()
        {
            Editor editor = new Editor();
            
            //TODO HACK: We don't want to read the header and bracket but sometimes we only need to skip over the bracket, so this solves the issue
            //This will also skip over comments
            string line = _reader.ReadLine();
            while (line != "{")
            {
                line = _reader.ReadLine();
            }
            line = _reader.ReadLine().Trim();
            
            while (line != "}")
            {
                if (line.StartsWith("//") || string.IsNullOrEmpty(line))
                {
                    line = _reader.ReadLine().Trim();
                    continue;
                }
                
                editor.AddProperty(new VProperty(line));
                line = _reader.ReadLine().Trim();
            }

            return editor;
        }

        #endregion

        #region VisGroups

        public VisGroups ReadVisGroups()
        {
            VisGroups visGroups = new VisGroups();
            
            //TODO HACK: We don't want to read the header and bracket but sometimes we only need to skip over the bracket, so this solves the issue
            //This will also skip over comments
            string line = _reader.ReadLine();
            while (line != "{")
            {
                line = _reader.ReadLine();
            }
            line = _reader.ReadLine();
            
            while (line != "}")
            {
                if (line.StartsWith("//") || string.IsNullOrEmpty(line))
                {
                    line = _reader.ReadLine();
                    continue;
                }
                
                visGroups.Groups.Add(ReadVisGroup());
                
                line = _reader.ReadLine();
            }

            return visGroups;
        }

        private VisGroup ReadVisGroup()
        {
            VisGroup visGroup = new VisGroup();
            _reader.ReadLine(); // {
            string line = _reader.ReadLine().Trim();
            
            while (line != "}")
            {
                if (line.StartsWith("//") || string.IsNullOrEmpty(line))
                {
                    line = _reader.ReadLine().Trim();
                    continue;
                }

                //Is this a child visgroup, or a property of this visgroup?
                if (line == "visgroup")
                {
                    visGroup.Groups.Add(ReadVisGroup());
                }
                else
                {
                    visGroup.AddProperty(new VProperty(line));
                }

                line = _reader.ReadLine().Trim();
            }

            return visGroup;
        }

        #endregion

        #region World Info

        public World ReadWorld()
        {
            World world = new World();
            
            //TODO HACK: We don't want to read the header and bracket but sometimes we only need to skip over the bracket, so this solves the issue
            //This will also skip over comments
            string line = _reader.ReadLine();
            while (line != "{")
            {
                line = _reader.ReadLine();
            }
            line = _reader.ReadLine();
            
            while (line != "}")
            {
                if (line.StartsWith("//") || string.IsNullOrEmpty(line))
                {
                    line = _reader.ReadLine();
                    continue;
                }

                switch (line.Trim())
                {
                    case "group":
                    {
                        world.Groups.Add(ReadGroup());
                    } break;
                    case "hidden":
                    {
                        world.HiddenClasses.Add(ReadHidden()); //TODO: Better implementation, Hidden should be its own class similar to group
                    } break;
                    case "solid":
                    {
                        world.Solids.Add(ReadSolid());
                    } break;
                    default:
                    {
                        world.AddProperty(new VProperty(line));
                    } break;
                }
                
                line = _reader.ReadLine();
            }

            return world;
        }

        #region Solids

        private Solid ReadSolid()
        {
            Solid solid = new Solid();
            _reader.ReadLine(); // {
            string line = _reader.ReadLine().Trim();
            
            while (line != "}")
            {
                if (line.StartsWith("//") || string.IsNullOrEmpty(line))
                {
                    line = _reader.ReadLine().Trim();
                    continue;
                }

                switch (line)
                {
                    case "side":
                    {
                        solid.Side = ReadSide();
                    } break;
                    case "editor":
                    {
                        solid.Editor = ReadEditor();
                    } break;
                    default:
                    {
                        solid.AddProperty(new VProperty(line));
                    } break;
                }

                line = _reader.ReadLine().Trim();
            }

            return solid;
        }

        private Side ReadSide()
        {
            Side side = new Side();
            _reader.ReadLine(); // {
            string line = _reader.ReadLine().Trim();
            
            while (line != "}")
            {
                if (line.StartsWith("//") || string.IsNullOrEmpty(line))
                {
                    line = _reader.ReadLine().Trim();
                    continue;
                }

                switch (line)
                {
                    case "dispinfo":
                    {
                        side.DisplacementInfo = ReadDisplacement();
                    } break;
                    default:
                    {
                        side.AddProperty(new VProperty(line));
                    } break;
                }

                line = _reader.ReadLine().Trim();
            }

            return side;
        }
        
        private Displacement ReadDisplacement()
        {
            Displacement displacement = new Displacement();
            _reader.ReadLine(); // {
            string line = _reader.ReadLine().Trim();
            
            while (line != "}")
            {
                if (line.StartsWith("//") || string.IsNullOrEmpty(line))
                {
                    line = _reader.ReadLine().Trim();
                    continue;
                }

                switch (line)
                {
                    case "allowed_verts":
                    {
                        _reader.ReadLine(); // {
                        string normalData = "";
                        while (line != "}")
                        {
                            line = _reader.ReadLine().Trim();
                            normalData += $"{line}\n";
                        }

                        displacement.Rows.ParseAlpha(normalData);
                    } break;
                    case "triangle_tags":
                    {
                        _reader.ReadLine(); // {
                        string normalData = "";
                        while (line != "}")
                        {
                            line = _reader.ReadLine().Trim();
                            normalData += $"{line}\n";
                        }

                        displacement.Rows.ParseAlpha(normalData);
                    } break;
                    case "alphas":
                    {
                        _reader.ReadLine(); // {
                        string normalData = "";
                        while (line != "}")
                        {
                            line = _reader.ReadLine().Trim();
                            normalData += $"{line}\n";
                        }

                        displacement.Rows.ParseAlpha(normalData);
                    } break;
                    case "offset_normals": //TODO:
                    {
                        _reader.ReadLine(); // {
                        string normalData = "";
                        while (line != "}")
                        {
                            line = _reader.ReadLine().Trim();
                            normalData += $"{line}\n";
                        }

                        displacement.Rows.ParseNormals(normalData);
                    } break;
                    case "offsets":
                    {
                        _reader.ReadLine(); // {
                        string normalData = "";
                        while (line != "}")
                        {
                            line = _reader.ReadLine().Trim();
                            normalData += $"{line}\n";
                        }

                        displacement.Rows.ParseOffsets(normalData);
                    } break;
                    case "distances":
                    {
                        _reader.ReadLine(); // {
                        string normalData = "";
                        while (line != "}")
                        {
                            line = _reader.ReadLine().Trim();
                            normalData += $"{line}\n";
                        }

                        displacement.Rows.ParseNormals(normalData);
                    } break;
                    case "normals":
                    {
                        _reader.ReadLine(); // {
                        string normalData = "";
                        while (line != "}")
                        {
                            line = _reader.ReadLine().Trim();
                            normalData += $"{line}\n";
                        }

                        displacement.Rows.ParseNormals(normalData);
                    } break;
                    default:
                    {
                        displacement.AddProperty(new VProperty(line));
                    } break;
                }

                line = _reader.ReadLine().Trim();
            }

            return displacement;
        }

#endregion

        #region Hidden
        
        private Hidden ReadHidden()
        {
            Hidden hidden = new Hidden();
            
            //TODO HACK: We don't want to read the header and bracket but sometimes we only need to skip over the bracket, so this solves the issue
            //This will also skip over comments
            string line = _reader.ReadLine();
            while (line != "{")
            {
                line = _reader.ReadLine();
            }
            line = _reader.ReadLine().Trim();
            
            while (line != "}")
            {
                if (line.StartsWith("//") || string.IsNullOrEmpty(line))
                {
                    line = _reader.ReadLine().Trim();
                    continue;
                }

                switch (line.Trim())
                {
                    case "Entity":
                    {
                        hidden.Class = ReadEntity();
                    } break;
                    case "Solid":
                    {
                        hidden.Class = ReadSolid();
                    } break;
                }
                
                line = _reader.ReadLine().Trim();
            }

            return hidden;
        }

        #endregion

        #region Groups

        private Group ReadGroup()
        {
            Group group = new Group();
            
            //TODO HACK: We don't want to read the header and bracket but sometimes we only need to skip over the bracket, so this solves the issue
            //This will also skip over comments
            string line = _reader.ReadLine();
            while (line != "{")
            {
                line = _reader.ReadLine();
            }
            line = _reader.ReadLine().Trim();
            
            while (line != "}")
            {
                if (line.StartsWith("//") || string.IsNullOrEmpty(line))
                {
                    line = _reader.ReadLine().Trim();
                    continue;
                }

                switch (line.Trim())
                {
                    default:
                    {
                        group.AddProperty(new VProperty(line));
                    } break;
                }
                
                line = _reader.ReadLine().Trim();
            }

            return group;
        }

        #endregion

        #endregion

        #region Entities

        public Entity ReadEntity()
        {
            Entity entity = new Entity();
            
            //TODO HACK: We don't want to read the header and bracket but sometimes we only need to skip over the bracket, so this solves the issue
            //This will also skip over comments
            string line = _reader.ReadLine();
            while (line != "{")
            {
                line = _reader.ReadLine();
            }
            line = _reader.ReadLine();
            
            while (line != "}")
            {
                if (line.StartsWith("//") || string.IsNullOrEmpty(line))
                {
                    line = _reader.ReadLine();
                    continue;
                }

                switch (line.Trim())
                {
                    case "editor":
                    {
                        ReadEditor();
                    } break;
                    case "hidden":
                    {
                        ReadHidden();
                    } break;
                    case "connections":
                    {
                        entity.Connections = ReadConnections();
                    } break;
                    case "solid":
                    {
                        entity.Solid = ReadSolid();
                    } break;
                    default:
                    {
                        entity.AddProperty(new VProperty(line));
                    } break;
                }
                
                line = _reader.ReadLine();
            }

            return entity;
        }

        private List<Connection> ReadConnections()
        {
            List<Connection> connections = new List<Connection>();
            
            //TODO HACK: We don't want to read the header and bracket but sometimes we only need to skip over the bracket, so this solves the issue
            //This will also skip over comments
            string line = _reader.ReadLine();
            while (line != "{")
            {
                line = _reader.ReadLine();
            }
            line = _reader.ReadLine().Trim(); //{
            
            while (line != "}")
            {
                if (line.StartsWith("//") || string.IsNullOrEmpty(line))
                {
                    line = _reader.ReadLine().Trim();
                    continue;
                }
                
                connections.Add(new Connection(line));
                line = _reader.ReadLine().Trim();
            }

            return connections;
        }

        #endregion
    }
}