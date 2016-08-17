package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.virtual.GameObject;
import foundation.softwaredesign.orbi.model.virtual.Position;
import foundation.softwaredesign.orbi.model.virtual.World;

import javax.enterprise.context.RequestScoped;
import java.math.BigDecimal;

import static java.util.Objects.nonNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
abstract class WorldAdapter {

    public abstract void scale(World world);

    protected Boolean isWorldOk(World world) {
        if (nonNull(world) && nonNull(world.getGameObjects()) && !world.getGameObjects().isEmpty()) {
            return true;
        }
        return false;
    }

    protected Boolean isPositionOk(Position position) {
        if (nonNull(position)) {
            if (nonNull(position.getZ())
                    && nonNull(position.getX())
                    && nonNull(position.getY())) {
                return true;
            }
        }
        return false;
    }

    protected Boolean isCubeOk(GameObject gameObject) {
        if (nonNull(gameObject) && nonNull(gameObject.getPosition())) {
            if (nonNull(gameObject.getPosition().getX())
                    && nonNull(gameObject.getPosition().getY())
                    && nonNull(gameObject.getPosition().getZ())) {
                return true;
            }
        }
        return false;
    }
}
