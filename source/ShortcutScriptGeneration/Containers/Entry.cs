﻿namespace Petrichor.ShortcutScriptGeneration.Containers
{
	public sealed class Entry : IEquatable<Entry>
	{
		public string Color { get; set; } = string.Empty;
		public string Decoration { get; set; } = string.Empty;
		public string ID { get; set; } = string.Empty;
		public List<Identity> Identities { get; set; } = new();
		public Identity LastIdentity { get; set; } = new();
		public string Pronoun { get; set; } = string.Empty;


		public Entry() { }
		public Entry( Entry other )
		{
			Color = other.Color;
			Decoration = other.Decoration;
			ID = other.ID;
			Identities = other.Identities;
			LastIdentity = other.LastIdentity;
			Pronoun = other.Pronoun;
		}
		public Entry( string id, List<Identity> identities )
		{
			ID = id;
			Identities = identities;
		}
		public Entry( string id, List<Identity> identities, Identity lastIdentity, string pronoun, string color, string decoration )
		{
			Color = color;
			Decoration = decoration;
			ID = id;
			Identities = identities;
			LastIdentity = lastIdentity;
			Pronoun = pronoun;
		}


		public static bool operator ==( Entry a, Entry b ) => a.Equals( b );

		public static bool operator !=( Entry a, Entry b ) => !a.Equals( b );

		public override bool Equals( object? obj )
		{
			if ( obj == null || GetType() != obj.GetType() )
			{
				return false;
			}
			return Equals( ( Entry ) obj );
		}

		public bool Equals( Entry? other )
		{
			if ( other is null )
			{
				return false;
			}
			return Color.Equals( other.Color ) && Decoration.Equals( other.Decoration ) && ID.Equals( other.ID ) && Identities.SequenceEqual( other.Identities ) && LastIdentity.Equals( other.LastIdentity ) && Pronoun.Equals( other.Pronoun );
		}

		public override int GetHashCode() => Color.GetHashCode() ^ Decoration.GetHashCode() ^ ID.GetHashCode() ^ Identities.GetHashCode() ^ LastIdentity.GetHashCode() ^ Pronoun.GetHashCode();
	}
}
