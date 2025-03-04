using Microsoft.Xna.Framework.Net;

namespace AppNamespace;

public interface IOnlineSyncTarget
{
	void startSynchronization();

	void endSynchronization();

	void prepareForSending();

	bool writeTransferRecord(PacketWriter writer);

	bool readTransferRecord(PacketReader reader);
}
