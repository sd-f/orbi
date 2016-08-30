package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.GameObject;
import foundation.softwaredesign.orbi.model.Player;
import foundation.softwaredesign.orbi.model.World;
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
    GameObjectRepository gameObjectRepository;
    @Inject
    WorldService worldService;
    @Inject
    UserService userService;

    private void saveGameObject(GameObject gameObject) {
        gameObject.setCreateDate(new Date());
        gameObjectRepository.saveAndFlush(gameObject);
    }

    public World craftGameObject(Player player) {
        userService.updatePosition(player.getGeoPosition());
        saveGameObject(player.getGameObjectToCraft());
        return worldService.getWorld(player.getGeoPosition());
    }
}
