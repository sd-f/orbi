package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.*;
import foundation.softwaredesign.orbi.persistence.repo.GameObjectRepository;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import java.util.Date;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class PlayerService {

    @Inject
    WorldService world;
    @Inject
    UserService user;
    @Inject
    InventoryService inventory;

    public World craft(Player player) {
        user.updatePosition(player.getGeoPosition());
        inventory.use(player.getGameObjectToCraft());
        player.getGameObjectToCraft().setIdentityId(user.getIdentity().getId());
        world.create(player.getGameObjectToCraft());
        return world.getWorld(player.getGeoPosition());
    }

    public World destroy(Player player) {
        world.delete(player.getSelectedObjectId());
        return world.getWorld(player.getGeoPosition());
    }

    public Inventory getInventory() {
        inventory.checkRestock();
        return inventory.getInventory();
    }

}
