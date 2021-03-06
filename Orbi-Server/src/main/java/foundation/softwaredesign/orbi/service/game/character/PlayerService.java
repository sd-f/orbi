package foundation.softwaredesign.orbi.service.game.character;

import foundation.softwaredesign.orbi.model.game.character.Character;
import foundation.softwaredesign.orbi.model.game.character.CharacterDevelopment;
import foundation.softwaredesign.orbi.model.game.character.Inventory;
import foundation.softwaredesign.orbi.model.game.character.Player;
import foundation.softwaredesign.orbi.model.game.gameobject.GameObject;
import foundation.softwaredesign.orbi.model.game.transform.Transform;
import foundation.softwaredesign.orbi.model.game.world.World;
import foundation.softwaredesign.orbi.service.auth.UserService;
import foundation.softwaredesign.orbi.service.game.gameobject.GameObjectService;
import foundation.softwaredesign.orbi.service.game.world.WorldService;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.ws.rs.NotFoundException;
import java.util.logging.Logger;

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
    @Inject
    GameObjectService gameObjectService;
    @Inject
    CharacterService characterService;

    public World craft(Player player) {
        characterService.updateTransform(player.getCharacter().getTransform());
        inventory.checkBasicInventoryAndRestock();
        inventory.use(player.getGameObjectToCraft());
        player.getGameObjectToCraft().setIdentityId(user.getIdentity().getId());
        world.create(player.getGameObjectToCraft());
        characterService.incrementXp(CharacterDevelopment.XP_CRAFT);
        return world.getWorld(player.getCharacter().getTransform().getGeoPosition());
    }

    public World destroy(Player player) {
        characterService.updateTransform(player.getCharacter().getTransform());
        Long id = player.getSelectedObjectId();
        try {
            GameObject object = gameObjectService.findById(id);
            inventory.addItem(object.getType().getPrefab(), new Long(1));
            inventory.checkForGiftChest(object);
            world.delete(object.getId());
            characterService.incrementXp(CharacterDevelopment.XP_DESTROY);
        } catch (NotFoundException ex) {
            Logger.getLogger(PlayerService.class.getName()).fine(ex.getMessage());
        }
        return world.getWorld(player.getCharacter().getTransform().getGeoPosition());
    }

    public Inventory getInventory() {
        inventory.checkBasicInventoryAndRestock();

        return inventory.getInventory();
    }

    /**
     *
     * @param newTransform initial or update position and rotation
     * @return
     */
    public Player update(Transform newTransform) {
        Character currentCharacter = characterService.updateTransform(newTransform);


        Player player = new Player();
        player.setCharacter(currentCharacter);
        return player;
    }

    public Player load() {
        Character currentCharacter = characterService.loadCurrent();
        Player player = new Player();
        player.setCharacter(currentCharacter);
        return player;
    }

}
