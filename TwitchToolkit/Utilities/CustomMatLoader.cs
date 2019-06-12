using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Utilities
{
    public static class CustomMatLoader
    {

        public static List<Material> MatsFromTexturesInFolder(string dirPath)
        {
            string path = "Textures/" + dirPath;
            return (from Texture2D tex in Resources.LoadAll(path, typeof(Texture2D))
                    select MaterialPool.MatFrom(tex)).ToList<Material>();
        }

        public static Material MatWithEnding(string dirPath, string ending)
        {
            Material material = (from mat in MaterialLoader.MatsFromTexturesInFolder(dirPath)
                                    where mat.mainTexture.name.ToLower().EndsWith(ending)
                                    select mat).FirstOrDefault<Material>();
            if (material == null)
            {
                Helper.Log("MatWithEnding: Dir " + dirPath + " lacks texture ending in " + ending);
                return BaseContent.BadMat;
            }
            return material;
        }

        //// Token: 0x0600621C RID: 25116 RVA: 0x002C7688 File Offset: 0x002C5A88
        //public static Material LoadMat(string matPath, int renderQueue = -1)
        //{
        //    Material material = (Material)Resources.Load("Textures/" + matPath, typeof(Material));
        //    if (material == null)
        //    {
        //        Helper.Log("Could not load material " + matPath, false);
        //    }
        //    CustomMatLoader.Request key = new CustomMatLoader.Request
        //    {
        //        path = matPath,
        //        renderQueue = renderQueue
        //    };
        //    Material material2;
        //    if (!CustomMatLoader.dict.TryGetValue(key, out material2))
        //    {
        //        var assembly = Assembly.GetAssembly(typeof(MatLoader));

        //        var type = assembly.GetType("Verse.MaterialAllocator");

        //        MethodInfo method = type.GetMethod("Create", BindingFlags.Static, null, CallingConventions.Any, new[] { typeof(Material) }, null);

        //        material2 = method.Invoke(method, new object[] { material }) as Material;

        //        //material2 = MaterialAllocator.Create(material);
        //        if (renderQueue != -1)
        //        {
        //            material2.renderQueue = renderQueue;
        //        }
        //        CustomMatLoader.dict.Add(key, material2);
        //    }
        //    return material2;
        //}

        //// Token: 0x04004088 RID: 16520
        //private static Dictionary<CustomMatLoader.Request, Material> dict = new Dictionary<CustomMatLoader.Request, Material>();

        //// Token: 0x02000FBF RID: 4031
        //private struct Request
        //{
        //    // Token: 0x0600621E RID: 25118 RVA: 0x002C772C File Offset: 0x002C5B2C
        //    public override int GetHashCode()
        //    {
        //        int seed = 0;
        //        seed = Gen.HashCombine<string>(seed, this.path);
        //        return Gen.HashCombineInt(seed, this.renderQueue);
        //    }

        //    // Token: 0x0600621F RID: 25119 RVA: 0x002C7756 File Offset: 0x002C5B56
        //    public override bool Equals(object obj)
        //    {
        //        return obj is CustomMatLoader.Request && this.Equals((CustomMatLoader.Request)obj);
        //    }

        //    // Token: 0x06006220 RID: 25120 RVA: 0x002C7771 File Offset: 0x002C5B71
        //    public bool Equals(CustomMatLoader.Request other)
        //    {
        //        return other.path == this.path && other.renderQueue == this.renderQueue;
        //    }

        //    // Token: 0x06006221 RID: 25121 RVA: 0x002C779C File Offset: 0x002C5B9C
        //    public static bool operator ==(CustomMatLoader.Request lhs, CustomMatLoader.Request rhs)
        //    {
        //        return lhs.Equals(rhs);
        //    }

        //    // Token: 0x06006222 RID: 25122 RVA: 0x002C77A6 File Offset: 0x002C5BA6
        //    public static bool operator !=(CustomMatLoader.Request lhs, CustomMatLoader.Request rhs)
        //    {
        //        return !(lhs == rhs);
        //    }

        //    // Token: 0x06006223 RID: 25123 RVA: 0x002C77B2 File Offset: 0x002C5BB2
        //    public override string ToString()
        //    {
        //        return string.Concat(new object[]
        //        {
        //            "MatLoader.Request(",
        //            this.path,
        //            ", ",
        //            this.renderQueue,
        //            ")"
        //        });
        //    }

        //    // Token: 0x04004089 RID: 16521
        //    public string path;

        //    // Token: 0x0400408A RID: 16522
        //    public int renderQueue;
        //}
    }
}
