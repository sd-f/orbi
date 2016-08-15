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
@RequestScoped
public class WorldAdapter {

    private Boolean isWorldOk(World world) {
        if (nonNull(world) && nonNull(world.getGameObjects()) && !world.getGameObjects().isEmpty()) {
            return true;
        }
        return false;
    }

    private Boolean isPositionOk(Position position) {
        if (nonNull(position)) {
            if (nonNull(position.getZ())
                    && nonNull(position.getX())
                    && nonNull(position.getY())) {
                return true;
            }
        }
        return false;
    }

    private Boolean isCubeOk(GameObject gameObject) {
        if (nonNull(gameObject) && nonNull(gameObject.getPosition())) {
            if (nonNull(gameObject.getPosition().getX())
                    && nonNull(gameObject.getPosition().getY())
                    && nonNull(gameObject.getPosition().getZ())) {
                return true;
            }
        }
        return false;
    }

    private BigDecimal scaleToVirtual(BigDecimal real, BigDecimal position, Integer scale) {
        BigDecimal newCoordinate = real.subtract(position);
        return newCoordinate.multiply(new BigDecimal(scale));
    }

    private BigDecimal scaleToReal(BigDecimal virtual, BigDecimal position, Integer scale) {
        BigDecimal newCoordinate = virtual.divide(new BigDecimal(scale));
        return newCoordinate.add(position);
    }

    public void convertPositionToVirtual(Position virtualPosition, Position referencePosition) {
        virtualPosition.setZ(
                scaleToVirtual(virtualPosition.getZ(), referencePosition.getZ(), 50000).multiply(BigDecimal.valueOf(-1))
        );
        virtualPosition.setY(
                scaleToVirtual(virtualPosition.getY(), referencePosition.getY(), 1)
        );
        virtualPosition.setX(
                scaleToVirtual(virtualPosition.getX(), referencePosition.getX(), 50000));
    }

    public void convertPositionToReal(Position realPosition, Position referencePosition) {
        realPosition.setZ(
                scaleToReal(realPosition.getZ(), referencePosition.getZ(), 50000).multiply(BigDecimal.valueOf(-1))
        );
        realPosition.setY(
                scaleToReal(realPosition.getY(), referencePosition.getY(), 1)
        );
        realPosition.setX(
                scaleToReal(realPosition.getX(), referencePosition.getX(), 50000));
    }

    public void convertToVirtual(World world, Position position) {
        if (isWorldOk(world) && isPositionOk(position)) {
            world.getGameObjects()
                    .stream()
                    .filter(this::isCubeOk)
                    .forEach(gameObject -> {
                        convertPositionToVirtual(gameObject.getPosition(), position);
                    });
        }
    }

    public void convertToReal(World world, Position position) {
        if (isWorldOk(world) && isPositionOk(position)) {
            world.getGameObjects()
                    .stream()
                    .filter(this::isCubeOk)
                    .forEach(gameObject -> {
                        convertPositionToReal(gameObject.getPosition(), position);
                    });
        }
    }
}
